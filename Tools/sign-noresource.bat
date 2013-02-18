ildasm %1 /out:temp.il 
ilasm temp.il /dll /key:Common\Feng.snk /out:%2
sn -vf %2

del temp.il