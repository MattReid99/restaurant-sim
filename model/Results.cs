public class Results {
  public float TotalRevenue;
  public int CustomerServed;
  public int CustomersRejected;
  public float AverageSatisfaction;
  public int Days;

  public Results AppendTo(Results existing) {
    existing.TotalRevenue += this.TotalRevenue;
    existing.CustomersServed += this.CustomerServed;
    existing.AverageSatisfaction += this.AverageSatisfaction;
    existing.Days += Days;
  }
}
