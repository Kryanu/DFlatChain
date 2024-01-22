using DFlatChain;
using System.Linq;

Blockchain blockchain = Blockchain.GetBlockchain();
Block currentBlock = new("Genesis");

Block MineBlock(ref Block current)
{
    current.SetHash();
    string prev = "";
    if(current.Hash != null)
    {
        prev = current.Hash;
    }
    blockchain.AppendChain(current);
    Block mined = current;
    current = new(prev);
    return mined;
}

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/viewPendingTransactions", () =>
{
    return currentBlock.Transactions;
}).WithName("ViewPendingTransactions");

app.MapGet("/addTransaction", (string sender, string receiver, decimal amount, string description) =>
{
    Transaction transaction = new(sender,receiver,amount,description);
    if(currentBlock.Transactions.Count < 512)
    {
        currentBlock.Transactions.Add(transaction);
    }
    return transaction;

}).WithName("AddTransaction");

app.MapGet("/addBlock", () =>
{
    return MineBlock(ref currentBlock);
    
}).WithName("AddBlock");

app.MapGet("/getBlockchain", () =>
{
    return blockchain.GetChain();
})
.WithName("GetBlockchain");

app.MapGet("/isChainValid", () =>
{
    return blockchain.IsChainValid(blockchain.GetChain(), currentBlock.PreviousHash, blockchain.GetChain().Length - 1);
}).WithName("getIsChainValid");

app.Run();

