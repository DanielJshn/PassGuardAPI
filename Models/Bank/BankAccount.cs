namespace apief
{
    public class BankAccount
    {
        public Guid id { get; set; } = Guid.NewGuid();
        public Guid bankAccountId { get; set; }
        public string? cardHolderName { get; set; }
        public string? cardNumber { get; set; }
        public string? expirationDateTimeStamp { get; set; }
        public string? cvv { get; set; }
        public string? pin { get; set; }
        public string? createdTime { get; set; }
        public string? modifiedTime { get; set; }
        public string? categoryId { get; set; }
    }
}