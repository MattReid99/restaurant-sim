public class Results {
  public float TotalRevenue;
  public int CustomerServed;
  public int CustomersRejected;
  public float AverageSatisfaction;
  public int Days;

  public void Append(Results existing) {
    this.TotalRevenue += existing.TotalRevenue;
    this.CustomersServed += existing.CustomerServed;
    this.AverageSatisfaction += existing.AverageSatisfaction;
    this.Days += existing.Days;
  }
}
