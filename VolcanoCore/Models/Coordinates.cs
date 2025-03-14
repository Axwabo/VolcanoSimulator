﻿namespace VolcanoCore.Models;

public readonly record struct Coordinates(int Latitude, int Longitude)
{

    public static Coordinates operator +(Coordinates left, Coordinates right)
        => new(left.Latitude + right.Latitude, left.Longitude + right.Longitude);

    public static Coordinates operator -(Coordinates left, Coordinates right)
        => new(left.Latitude - right.Latitude, left.Longitude - right.Longitude);

    public static Coordinates operator *(Coordinates coordinates, int scalar)
        => new(coordinates.Latitude * scalar, coordinates.Longitude * scalar);

    public static Coordinates operator /(Coordinates coordinates, int scalar)
        => new(coordinates.Latitude / scalar, coordinates.Longitude / scalar);

    public static double DistanceSquared(Coordinates left, Coordinates right)
        => Math.Pow(left.Latitude - right.Latitude, 2) + Math.Pow(left.Longitude - right.Longitude, 2);

}
