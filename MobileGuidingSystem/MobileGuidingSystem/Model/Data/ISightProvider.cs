using System.Collections.Generic;

namespace MobileGuidingSystem.Model.Data
{
    internal interface ISightProvider {
        List<ISight> Sights { get; }
    }
}