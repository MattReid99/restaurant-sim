using System;

public static class ExtensionMethods {
    private static Random rand = new Random();
    public static double GetRandomNumber(double min, double max) {
        return rand.NextDouble() * (max - min) + min;
    } 
}