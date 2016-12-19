using System;
using System.Collections.Generic;
using Windows.Devices.Geolocation;

namespace MobileGuidingSystem.Model.Data
{
    public interface ISight
    {
        string Name { get; }
        string Description { get; }
        List<String> ImagePaths { get; }
        Geopoint Position { get; }
        string Address { get; }
    }
}