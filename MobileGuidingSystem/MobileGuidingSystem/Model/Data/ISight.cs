using System;
using System.Collections.Generic;
using Windows.Devices.Geolocation;
using Windows.Storage.Streams;

namespace MobileGuidingSystem.Model.Data
{
    public interface ISight
    {
        string Name { get; }
        string Description { get; }
        List<String> ImagePaths { get; }
        Geopoint Position { get; }
        string Address { get; }

        List<RandomAccessStreamReference> StreamReferences { get; }
        RandomAccessStreamReference Icon { get; }
    }
}