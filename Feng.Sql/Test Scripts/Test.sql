-- 运用此程序集实现的不同 SQL 对象的查询示例

-----------------------------------------------------------------------------------------
-- 存储过程
-----------------------------------------------------------------------------------------
-- exec StoredProcedureName


-----------------------------------------------------------------------------------------
-- 用户定义的函数
-----------------------------------------------------------------------------------------
-- select dbo.FunctionName()


-----------------------------------------------------------------------------------------
-- 用户定义的类型
-----------------------------------------------------------------------------------------
-- CREATE TABLE test_table (col1 UserType)
-- go
--
-- INSERT INTO test_table VALUES (convert(uri, 'Instantiation String 1'))
-- INSERT INTO test_table VALUES (convert(uri, 'Instantiation String 2'))
-- INSERT INTO test_table VALUES (convert(uri, 'Instantiation String 3'))
--
-- select col1::method1() from test_table



-----------------------------------------------------------------------------------------
-- 用户定义的类型
-----------------------------------------------------------------------------------------
-- select dbo.AggregateName(Column1) from Table1


SELECT  dbo.Concatenate(箱号) from 业务备案_普通箱
SELECT  dbo.Concatenate(DISTINCT 箱号) from 业务备案_普通箱

SELECT  箱号 from 业务备案_普通箱