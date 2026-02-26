using UnityEngine;
using System;

public enum MerchantType
{
    ArrowMerchant,
    FoodMerchant
}

// ===== RUOKA (Tehtävä 2) =====
public enum PaaraakaAine
{
    nautaa,
    kanaa,
    kasviksia
}

public enum Lisuke
{
    perunaa,
    riisiä,
    pastaa
}

public enum Kastike
{
    curry,
    hapanimelä,
    pippuri,
    chili
}

// ===== NUOLI (Tehtävä 3) =====
public enum Karki
{
    puu,
    teräs,
    timantti
}

public enum Pera
{
    lehti,
    kanansulka,
    kotkansulka
}

public class MerchantInfo : MonoBehaviour
{
    [Header("Basic Info")]
    public string merchantName;
    public MerchantType merchantType;

    // ================= NUOLEN HINTA =================
    public double GetArrowPrice(int karkiIndex, int peraIndex, int varrenPituus)
    {
        double hinta = 0;

        Karki karki = (Karki)karkiIndex;
        Pera pera = (Pera)peraIndex;

        switch (karki)
        {
            case Karki.puu: hinta += 3; break;
            case Karki.teräs: hinta += 5; break;
            case Karki.timantti: hinta += 50; break;
        }

        switch (pera)
        {
            case Pera.lehti: hinta += 0; break;
            case Pera.kanansulka: hinta += 1; break;
            case Pera.kotkansulka: hinta += 5; break;
        }

        hinta += varrenPituus * 0.05;

        return hinta;
    }

    // ================= RUOKA =================
    public int GetFoodPrice(int paaIndex)
    {
        PaaraakaAine paa = (PaaraakaAine)paaIndex;

        switch (paa)
        {
            case PaaraakaAine.nautaa: return 25;
            case PaaraakaAine.kanaa: return 20;
            case PaaraakaAine.kasviksia: return 15;
        }

        return 0;
    }

    public int GetFoodHeal(int paaIndex)
    {
        PaaraakaAine paa = (PaaraakaAine)paaIndex;

        switch (paa)
        {
            case PaaraakaAine.nautaa: return 25;
            case PaaraakaAine.kanaa: return 18;
            case PaaraakaAine.kasviksia: return 12;
        }

        return 0;
    }

    // ===== Enum nimet UI:ta varten =====
    public string[] GetKarkiNames() => Enum.GetNames(typeof(Karki));
    public string[] GetPeraNames() => Enum.GetNames(typeof(Pera));
    public string[] GetPaaraakaNames() => Enum.GetNames(typeof(PaaraakaAine));
    public string[] GetLisukeNames() => Enum.GetNames(typeof(Lisuke));
    public string[] GetKastikeNames() => Enum.GetNames(typeof(Kastike));
}