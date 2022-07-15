using System;

public class Results {
  public float TotalRevenue;
  public int CustomersServed;
  public int CustomersRejected;
  public float AverageSatisfaction;
  public float Popularity;
  public int Days;

  public void Append(Results existing) {
    this.TotalRevenue += existing.TotalRevenue;
    this.CustomersServed += existing.CustomersServed;
    this.AverageSatisfaction += existing.AverageSatisfaction;
    this.Days += existing.Days;
    this.Popularity = existing.Popularity;
  }

    public override string ToString()
    {
        string formatStr = "revenue: {0}   served: {1}   rejected: {2}" +
          "   satisfaction: {3}    population: {4}    days: {5}";

        return String.Format(formatStr, (TotalRevenue/Days).ToString("N0"), CustomersServed/Days, 
          ((float)(CustomersRejected)/Days).ToString("F3"), (AverageSatisfaction/Days).ToString("F2"), Popularity.ToString("F4"), Days);
    }
}
