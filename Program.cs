using DFlatChain;
using System.Linq;

Blockchain blockchain = Blockchain.GetBlockchain();
Block currentBlock = new("Genesis");
ChainSystem.StartUp();


var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/viewPendingTransactions", () => {
    return currentBlock.Transactions;
}).WithName("ViewPendingTransactions");

app.MapGet("/addTransaction", (string sender, string receiver, decimal amount, string description) => {
    //TODO:Add validation for description length <= 20
    //TODO:Add validation for amount length <= 10

    Transaction transaction = new(sender, receiver, amount, description);
    if (currentBlock.Transactions.Count < 512) {
        currentBlock.Transactions.Add(transaction);
    }
    return transaction;

}).WithName("AddTransaction");

app.MapGet("/addBlock", () => {
    currentBlock.PrepareBlockFormat();
    return ChainSystem.MineBlock(ref currentBlock, ref blockchain);

}).WithName("AddBlock");

app.MapGet("/getBlockchain", () => {
    return blockchain.GetChain();
})
.WithName("GetBlockchain");

app.MapGet("/isChainValid", () => {
    return blockchain.IsChainValid(blockchain.GetChain(), currentBlock.PreviousHash, blockchain.GetChain().Length - 1);
}).WithName("getIsChainValid");

app.MapGet("/SaveChainToFile", () => {
    try {
        ChainSystem.SaveChainToFile(blockchain);
        return "Success";
    } catch {
        return "Failed";
    }
}).WithName("SaveChainToFile");
//To remove when serialisation is done
app.MapGet("testCalculateMerkleHash", () => {
    
    return Merkle.GenerateTree(currentBlock.Transactions.ToArray());

}).WithName("calcHash");

app.Run();

