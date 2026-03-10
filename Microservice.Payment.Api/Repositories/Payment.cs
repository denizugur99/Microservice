using MassTransit;

namespace Microservice.Payment.Api.Repositories
{
    public class Payment
    {
        public Guid Id { get; set; }
        public Guid UserId {  get; set; }
        public string OrderCode { get; set; }
        public DateTimeOffset Created {  get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }

        public Payment(Guid UserId,string orderCode,decimal amount)
        {
            Create(UserId,orderCode,amount);
        }

        public void Create(Guid UserId,string orderCode,decimal amount)
        {
            Id = NewId.NextSequentialGuid();
            Created = DateTimeOffset.UtcNow;
            Status = PaymentStatus.Pending;
            this.UserId = UserId;
            this.OrderCode = orderCode;
            this.Amount = amount;
        }

        public void SetPaymentStatus(PaymentStatus status)
        {
            Status = status;
        }

    }
    public enum PaymentStatus {
    Succes=1,
    Failed=2,
    Pending= 3

    }
}
