# -*- coding: UTF-8 -*-
import clr
clr.AddReference("System.Windows.Forms");
import System;

def walk(folder):
  for file in System.IO.Directory.GetFiles(folder, '*.py'):
    yield file
  for folder in System.IO.Directory.GetDirectories(folder):
    for file in walk(folder): yield file
  
folder = System.IO.Path.GetDirectoryName(__file__) + "\\PythonScript\\";

#pygments_files = list(walk(IO.Path.Combine(folder, 'pygments')))
#pygments_dependencies = list(walk(IO.Path.Combine(folder, 'pygments_dependencies')))

#all_files = pygments_files + pygments_dependencies
#all_files.append(IO.Path.Combine(folder, 'devhawk_formatter.py'))

all_files = list(walk(folder))
#all_files.remove(__file__);

#for i in all_files:
#  System.Windows.Forms.MessageBox.Show(i);
clr.CompileModules(System.IO.Path.Combine(folder, "..\\reference\\PythonScript.dll"), *all_files)
