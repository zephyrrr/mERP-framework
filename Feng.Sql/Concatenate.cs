using System;
using System.Data.Sql;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.IO;
using System.Text;

/* sp_configure 'show advanced options', 1;
GO
RECONFIGURE;
GO
sp_configure 'clr enabled', 1;
GO
RECONFIGURE;
GO
 * Sample: SELECT  dbo.Concatenate(箱号) from 业务备案_普通箱
 * SELECT  dbo.Concatenate(DISTINCT 箱号) from 业务备案_普通箱 (不同的箱号)
 * */

/// <summary>
/// 
/// </summary>
[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedAggregate(
    //use CLR serialization to serialize the intermediate result. 
    Format.UserDefined, 
    //Optimizer property: 
    IsInvariantToNulls=true,
    //Optimizer property: 
    IsInvariantToDuplicates=true,
    //Optimizer property: 
    IsInvariantToOrder=false,
    //Maximum size in bytes of persisted value: 
    MaxByteSize=8000)
]
public class Concatenate : IBinarySerialize 
{
    /// <summary> 
    /// Variable holds intermediate result of the concatenation 
    /// </summary> 
    private StringBuilder intermediateResult; 

    /// <summary> 
    /// Initialize the internal data structures 
    /// </summary> 
    public void Init()
    {
        intermediateResult = new StringBuilder(); 
    }

    /// <summary> 
    /// Accumulate the next value, nop if the value is null 
    /// </summary> 
    /// <param name="value"></param> 
    public void Accumulate(SqlString value) 
    { 
        if(value.IsNull) 
        { 
            return; 
        } 
        intermediateResult.Append(value.Value).Append(','); 
    } 


     /// <summary> 
    /// Merge the partially computed aggregate with this aggregate. 
    /// </summary> 
    /// <param name="other"></param> 
    public void Merge( Concatenate other) 
    { 
        intermediateResult.Append(other.intermediateResult); 
    } 


    /// <summary> 
    /// Called at end of aggregation, to return results. 
    /// </summary> 
    /// <returns></returns> 
    public SqlString Terminate() 
    { 
        string output = string.Empty; 
        //Delete the trailing comma, if any .
        if (intermediateResult != null && intermediateResult.Length > 0) 
            output = intermediateResult.ToString(0, intermediateResult.Length-1); 
        return new SqlString(output); 
    } 

    public void Read(BinaryReader r) 
    { 
        intermediateResult = new StringBuilder(r.ReadString()); 
    } 

    public void Write(BinaryWriter w) 
    { 
        w.Write(intermediateResult.ToString()); 
    } 


}
