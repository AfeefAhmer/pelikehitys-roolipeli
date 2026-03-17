using System.Collections.Generic;
using System.Linq;

public class Reppu
{
    private List<Tavara> tavarat = new List<Tavara>();

    public int MaxMaara { get; }
    public double MaxPaino { get; }
    public double MaxTilavuus { get; }

    public Reppu(int maxMaara, double maxPaino, double maxTilavuus)
    {
        MaxMaara = maxMaara;
        MaxPaino = maxPaino;
        MaxTilavuus = maxTilavuus;
    }

    public int TavaroidenMaara => tavarat.Count;
    public double NykyPaino => tavarat.Sum(t => t.Paino);
    public double NykyTilavuus => tavarat.Sum(t => t.Tilavuus);
    public List<Tavara> GetItems()
    {
        return tavarat;
    }

    public bool Lisaa(Tavara tavara)
    {
        if (TavaroidenMaara + 1 > MaxMaara)
            return false;

        if (NykyPaino + tavara.Paino > MaxPaino)
            return false;

        if (NykyTilavuus + tavara.Tilavuus > MaxTilavuus)
            return false;

        tavarat.Add(tavara);
        return true;
    }
}