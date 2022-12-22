CREATE TRIGGER modifiedAt
ON [Order]
AFTER INSERT, UPDATE
AS
	UPDATE [Order] SET ModifiedAt = (SELECT getdate())
	FROM [Order] x
		INNER JOIN inserted y
		ON x.Id = y.Id
GO
