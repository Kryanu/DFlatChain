using DFlatChain;
using System.Linq;

Blockchain blockchain = Blockchain.GetBlockchain();
Block currentBlock = new("Genesis");

Transaction AddTransaction(string sender, string receiver, decimal amount, string description) {
    
    if (amount.ToString().Length > 10) {
        throw new Exception("Amount is too large");
    }
    if (description.Length > 20) {
        throw new Exception("Description is too long");
    }

    Transaction transaction = new(sender, receiver, amount, description);
    if (currentBlock.Transactions.Count < 512) {
        currentBlock.Transactions.Add(transaction);
    }else {
        //TODO: Mineblock
    }
    return transaction;
}

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
    return AddTransaction(sender, receiver, amount, description);

}).WithName("AddTransaction");

app.MapGet("/addBlock", () => {
    byte[] test = Helpers.Compress(currentBlock.PrepareBlockFormat());   
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


app.Run();

