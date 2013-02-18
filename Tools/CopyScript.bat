mkdir .\Reference\Script
cd PythonScript
if exist .\PythonScript\pythonFiles.txt del .\PythonScript\pythonFiles.txt>nul
dir /s /b .\PythonScript\*.py > pythonFiles.txt
for /f %%i in (pythonFiles.txt ) do copy %%i ..\Reference\Script
del .\PythonScript\pythonFiles.txt>nul

.\Reference\ipy.exe .\build_python.py

IF ERRORLEVEL 1 pause