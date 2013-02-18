import clr;
clr.AddReference("System");
clr.AddReference("System.Data");

from System import *
from System.Collections.Generic import *
from System.Text import *
from System.Data.SqlClient import *
from System.IO import *
from System.Net import *
	
class Program(object):
	def __init__(self):
		self._backupDbName = "jkhd2"
		self._ftpServerURI = "192.168.0.10:8021" # FTP server
		self._ftpUserID = "nbzs" # FTP Username
		self._ftpPassword = "qazwsxedc" #FTP Password
		self._strCon = "Data Source=192.168.0.10,8033;Initial Catalog=master;User ID=sa;Password=qazwsxedc" # Change SQLDBSERVER to the name of the SQL Server you are using
		self._drive = "D" # The local drive to save the backups to 
		self._LogFile = "SQLBackup.log" # The location on the local Drive of the log files.
		self._backupDir = ".\\Backup\\"
		self._backupFtpDir = "/SqlBackup/"
		self._DaysToKeep = 31 # Number of days to keep the daily backups for
		self._DayOfWeekToKeep = DayOfWeek.Sunday # Specify which daily backup to keep
		
	def Main(self, args):
		self._fnLog = self.RotateLog(FileInfo(self._LogFile), self._DaysToKeep)
		self.WriteLog("Starting Weekly Backup.", self._fnLog)
		self.Backup()
		self.WriteLog("Daily BackupFinished.", self._fnLog)

	def Backup(self):
		# need to specify here which databases you do not want to back up.
		#SqlCommand comSQL = new SqlCommand("select name from sysdatabases where name not in('tempdb','model','Northwind','AdventureWorks','master') order by name ASC", new SqlConnection(strCon)); 
		comSQL = SqlCommand("select name from sysdatabases where name in('" + self._backupDbName + "') order by name ASC", SqlConnection(self._strCon))
		comSQL.Connection.Open()
		dr = comSQL.ExecuteReader()
		while dr.Read():
			self.WriteLog("Backing Up Database - " + dr["name"], self._fnLog)
			if DateTime.Now.DayOfWeek != self._DayOfWeekToKeep:
				self.WriteLog("Deleting Backup from " + self._DaysToKeep.ToString() + " days ago", self._fnLog)
				oldfn = FileInfo(self._backupDir + dr["name"] + "\\" + dr["name"] + "_full_" + DateTime.Now.Subtract(TimeSpan.FromDays(14)).ToString("yyyyMMdd") + ".Bak")
				#FTPDeleteFile(new Uri("ftp://" + ftpServerURI + "/" + (string)dr["name"] + "/" + oldfn.Name), new NetworkCredential(ftpUserID, ftpPassword));
			else:
                                self.WriteLog("Keeping Weekly Backup.", self._fnLog)
			fn = FileInfo(self._backupDir + dr["name"] + "\\" + dr["name"] + "_full_" + DateTime.Now.ToString("yyyyMMdd") + ".Bak")
			if File.Exists(fn.FullName):
				self.WriteLog("Deleting Backup Because it Already Exists.", self._fnLog)
				File.Delete(fn.FullName)
			Directory.CreateDirectory(fn.DirectoryName)
			comSQL2 = SqlCommand("BACKUP DATABASE @db TO DISK = @fn;", SqlConnection(self._strCon))
			comSQL2.CommandTimeout = 360
			comSQL2.Connection.Open()
			comSQL2.Parameters.AddWithValue("@db", dr["name"])
			comSQL2.Parameters.AddWithValue("@fn", fn.FullName)
			self.WriteLog("Starting Backup", self._fnLog)
			comSQL2.ExecuteNonQuery()
			self.WriteLog("Backup Succeeded.", self._fnLog)
			comSQL2.Connection.Close()
			
			self.WriteLog("Uploading Backup to FTP server", self._fnLog)
			self.FTPDeleteFile(Uri("ftp://" + self._ftpServerURI + self._backupFtpDir + dr["name"] + "/" + fn.Name), NetworkCredential(self._ftpUserID, self._ftpPassword))
			if self.FTPUploadFile("ftp://" + self._ftpServerURI + self._backupFtpDir + dr["name"], "/" + fn.Name, fn, NetworkCredential(self._ftpUserID, self._ftpPassword)):
				self.WriteLog("Upload Succeeded", self._fnLog)
			else:
				self.WriteLog("Upload Failed", self._fnLog)
		comSQL.Connection.Close()

	def FTPDeleteFile(self, serverUri, Cred):
		retVal = True
		response = None
		try:
			request = WebRequest.Create(serverUri)
			request.Method = WebRequestMethods.Ftp.DeleteFile
			request.Credentials = Cred
			response = request.GetResponse()
			response.Close()
		except Exception, ex:
			if ex.Message != "The remote server returned an error: (550) File unavailable (e.g., file not found, no access).":
				Console.WriteLine("Error in FTPDeleteFile - " + ex.Message)
				if response != None:
					response.Close()
				retVal = False
		finally:
                        return retVal

	def FTPUploadFile(self, serverPath, serverFile, LocalFile, Cred):
		retVal = True
		response = None
		try:
			self.FTPMakeDir(Uri(serverPath + "/"), Cred)
			request = WebRequest.Create(serverPath + serverFile)
			request.Method = WebRequestMethods.Ftp.UploadFile
			request.Credentials = Cred
			buffer = Array.CreateInstance(Byte, 10240) # Read/write 10kb
			with FileStream(LocalFile.ToString(), FileMode.Open) as sourceStream:
                                with request.GetRequestStream() as requestStream:
                                        bytesRead = 1;
                                        while (bytesRead > 0):
                                                bytesRead = sourceStream.Read(buffer, 0, buffer.Length);
                                                requestStream.Write(buffer, 0, bytesRead);
   
                                response = request.GetResponse();
                                response.Close();
		except Exception, ex:
			Console.WriteLine("Error in FTPUploadFile - " + ex.Message)
			if response != None:
				response.Close()
			retVal = False
		finally:
                        return retVal

	def FTPMakeDir(self, serverUri, Cred):
		retVal = False
		response = None
		try:
			ar = serverUri.ToString().Split('/')
			makeDirUri = ar[0] + "//" + ar[2] + "/"
			i = 3
			while i < ar.GetUpperBound(0):
				makeDirUri += ar[i] + "/"
				request = WebRequest.Create(Uri(makeDirUri))
				request.KeepAlive = True
				request.Method = WebRequestMethods.Ftp.MakeDirectory
				request.Credentials = Cred
				try:
					response = request.GetResponse()
				except Exception, ex:
					if ex.Message != "The remote server returned an error: (550) File unavailable (e.g., file not found, no access).":
						retVal = False
				finally:
                                        i += 1
		except Exception, ex:
			Console.WriteLine("Error in FTPMakeDir - " + ex.Message)
			retVal = False
			if response != None:
				response.Close()
		finally:
                        return retVal

	def RotateLog(self, LogFileName, Days):
		fNew = LogFileName.Directory.ToString() + DateTime.Now.ToString("\\\\yyyyMMdd_") + LogFileName.Name
		fOld = LogFileName.Directory.ToString() + DateTime.Now.Subtract(TimeSpan.FromDays(Days)).ToString("\\\\yyyyMMdd_") + LogFileName.Name
		fOldRecycler = "C:\\RECYCLER\\" + DateTime.Now.Subtract(TimeSpan.FromDays(Days)).ToString("yyyyMMdd_") + LogFileName.Name
		if File.Exists(fOld):
			self.WriteLog("Deleting LogFile - " + fOld + " because it is over " + Days.ToString() + " Days old", fNew)
			File.Move(fOld, fOldRecycler)
		return fNew


	def WriteLog(self, s, fn):
		File.AppendAllText(fn, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss:ffff") + " - " + s + Environment.NewLine)

if __name__ == "__main__":
	p = Program();
	p.Main('');
