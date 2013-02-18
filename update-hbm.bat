rem .\Reference\ipy.exe Tools\generateEntityClass.py

cd .\Reference
mkdir hbm

del /Q ..\Reference\hbm\*
..\Reference\ADInfosUtil.exe -hbm Feng.Windows.Model
copy /B /Y ..\Reference\hbm\* ..\Feng.Windows.Model\Domain.hbm.xml

del /Q ..\Reference\hbm\*
..\Reference\ADInfosUtil.exe -hbm Feng.Gps
copy /B /Y ..\Reference\hbm\* ..\Feng.Gps\Domain.hbm.xml

goto end


del /Q ..\Reference\hbm\*
..\Reference\ADInfosUtil.exe -hbm Feng.Example
copy /B /Y ..\Reference\hbm\* ..\Feng.Example\Domain.hbm.xml

:end
del /Q ..\Reference\hbm\*
rmdir ..\Reference\hbm\

cd ..

IF ERRORLEVEL 1 pause