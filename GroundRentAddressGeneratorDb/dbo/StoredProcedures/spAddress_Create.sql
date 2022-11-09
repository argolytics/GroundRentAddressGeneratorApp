CREATE PROCEDURE [dbo].[spAddress_Create]
	@AccountId nvarchar(50),
	@IsRedeemed BIT,
	@Id INT OUTPUT
AS
begin
	set nocount on;

	insert into dbo.[Address](
	[AccountId],
	[IsRedeemed])

	values(
	@AccountId,
	@IsRedeemed)

	SET @Id = SCOPE_IDENTITY();
end
