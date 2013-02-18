#in Shell, execfile(r"..\exportViews.py");
#Create Directory "Dbs" in "Reference"(CurrentDirectory)

import clr
clr.AddReference("Feng.Data")
clr.AddReference("Feng.Base")
import Feng;
import System;

def exportviews():
	dt = Feng.Data.DbHelper.GetViewFuncProcTrigs();
	#object_id, name, definition, Type
	for row in dt.Rows:
		fileName = "Dbs\\" + row["name"] + "." + row["type"];
		with System.IO.StreamWriter(fileName, System.Text.Encoding.UTF8) as sw:
			sw.Write(row["definition"]);

if __name__ == "__main__" or __name__ == "__builtin__":
	exportviews();
