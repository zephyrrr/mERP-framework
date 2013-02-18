# -*- coding: UTF-8 -*-
import clr
clr.AddReference("System.Data");
clr.AddReference("Feng.Data");
clr.AddReference("Feng.Security");
import System;
import Feng;

def upload_python():
    folder = System.IO.Path.GetDirectoryName(__file__) + "\\PythonScript\\";
    all_files = list(walk(folder, '*.py'))
    upload(all_files, "Python_", 1, True, True);

def upload_report():
    folder = System.IO.Path.GetDirectoryName(__file__) + "\\Hd.Report\\";
    all_files_dataset = list(walk(folder, '*.xsd'))
    upload(all_files_dataset, "Dataset_", 3, True, True);
    all_files_report = list(walk(folder, '*.rpt'))
    upload(all_files_report, "Report_", 2, False, True);
    all_files_report = list(walk(folder, '*.rdlc'))
    upload(all_files_report, "Report_", 5, True, True);
    
def upload_dll():
    folder = System.IO.Path.GetDirectoryName(__file__) + "\\Reference\\";
    all_files = list(walk(folder, 'hd.*.dll'));
    upload(all_files, "Assembly_", 4, False, True);

def walk(folder, extention):
  for file in System.IO.Directory.GetFiles(folder, extention):
    yield file
  for folder in System.IO.Directory.GetDirectories(folder):
    for file in walk(folder, extention): yield file

def upload(all_files, prefix, resourceType, useStringReader, persistLocal):
  for i in all_files:
    fileName = System.IO.Path.GetFileName(i);
    if useStringReader:
        sr = System.IO.StreamReader(i);
        content = sr.ReadToEnd();
        sr.Close();
        contentType = "System.String";
    else:
        sr = System.IO.FileStream(i, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
        byte = System.Array.CreateInstance(System.Byte, int(sr.Length));
        sr.Read(byte, 0, sr.Length); 
        content = System.Convert.ToBase64String(byte);
        sr.Close();
        contentType = "Syste.Byte[]";
        
    sql = "DELETE AD_Resource WHERE ResourceName = '" + fileName + "' AND ResourceType = " + str(resourceType);
    print(sql);
    Feng.Data.DbHelper.Instance.ExecuteNonQuery(sql);
    sql = "INSERT INTO AD_Resource \
      ([Name],[Version],[ResourceType],[ResourceName],[Content],[Md5], [PersistLocal], [IsActive],[ClientId],[OrgId],[CreatedBy],[Created]) \
      VALUES (" + "@name" + \
      ",1, @resourcetype, @fileName, @content," + \
      "@md5, @persist, " + \
      "'True', 0, 0, 'system', GETDATE())";
    print(sql);
    cmd = System.Data.SqlClient.SqlCommand(sql);
    cmd.Parameters.AddWithValue("@name", prefix + fileName);
    cmd.Parameters.AddWithValue("@resourcetype", resourceType);
    cmd.Parameters.AddWithValue("@fileName", fileName);
    cmd.Parameters.AddWithValue("@content", content);
    cmd.Parameters.AddWithValue("@md5", Feng.Cryptographer.Md5(content));
    cmd.Parameters.AddWithValue("@persist", persistLocal);
    Feng.Data.DbHelper.Instance.ExecuteNonQuery(cmd);
    
if __name__ == "__main__" or __name__ == "__builtin__":
    upload_dll();
    upload_python();
    upload_report();
    
	

    

   
