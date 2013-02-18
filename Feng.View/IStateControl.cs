namespace Feng
{
    /// <summary>
    /// ×´Ì¬
    /// </summary>
    [System.Flags]
    public enum StateType
    {
        ///// <summary>
        ///// 
        ///// </summary>
        //Invalid = -1,
        /// <summary>
        /// Êý¾Ý¿Õ
        /// </summary>
        None = 0x1,
        /// <summary>
        /// ÏÔÊ¾
        /// </summary>
        View = 0x2,
        /// <summary>
        /// Ìí¼Ó
        /// </summary>
        Add = 0x4,
        /// <summary>
        /// ±à¼­
        /// </summary>
        Edit = 0x8,
        /// <summary>
        /// É¾³ý
        /// </summary>
        Delete = 0x10,
    };

    /// <summary>
    /// ×´Ì¬¿Ø¼þ
    /// </summary>
    public interface IStateControl
    {
        /// <summary>
        /// ÉèÖÃ×´Ì¬
        /// </summary>
        void SetState(StateType state);
    }
}