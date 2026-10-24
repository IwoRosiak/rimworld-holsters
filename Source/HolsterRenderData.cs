using System.Collections.Generic;
using Verse;

namespace Holsters
{
    public class HolsterRenderData : IExposable
    {
        public Dictionary<Rot4, HolsterConfiguration> Configuration = new Dictionary<Rot4, HolsterConfiguration>();

        public Dictionary<Rot4, HolsterConfiguration> SideConfiguration = new Dictionary<Rot4, HolsterConfiguration>();


        public Dictionary<string, float> BodyOffsetsModifiers;
        public Dictionary<string, float> BodyOffsetsSideModifiers;

        public HolsterRenderData() { }

        public HolsterRenderData(HolsterRenderData copy)
        {
            Configuration = new Dictionary<Rot4, HolsterConfiguration>();
            foreach (KeyValuePair<Rot4, HolsterConfiguration> pair in copy.Configuration)
            {
                Configuration.Add(pair.Key, pair.Value.Copy());
            }

            SideConfiguration = new Dictionary<Rot4, HolsterConfiguration>();
            foreach (KeyValuePair<Rot4, HolsterConfiguration> pair in copy.SideConfiguration)
            {
                SideConfiguration.Add(pair.Key, pair.Value.Copy());
            }

            BodyOffsetsModifiers = new Dictionary<string, float>(copy.BodyOffsetsModifiers);
            BodyOffsetsSideModifiers = new Dictionary<string, float>(copy.BodyOffsetsSideModifiers);

        }

        public Dictionary<string, float> AllBodyOffsetsModifiers 
        { 
            set
            {
                BodyOffsetsModifiers = new Dictionary<string, float>(value);
                BodyOffsetsSideModifiers = new Dictionary<string, float>(value);
            } 
        }

        public float GetBodyModifier(string bodyTypeDefName, bool side)
        {
            if (BodyOffsetsModifiers == null)
                BodyOffsetsModifiers = GetDefaultBodyModifiersDictionary();
            if (BodyOffsetsSideModifiers == null)
                BodyOffsetsSideModifiers = GetDefaultBodyModifiersDictionary();

            if (bodyTypeDefName.NullOrEmpty())
                return 0f;

            bodyTypeDefName = bodyTypeDefName.ToLower();
            Dictionary<string, float> dict = side ? BodyOffsetsSideModifiers : BodyOffsetsModifiers;

            if (dict.ContainsKey(bodyTypeDefName))
                return dict[bodyTypeDefName];

            float defaultValue = 0;
            dict.Add(bodyTypeDefName, defaultValue);

            return defaultValue;
        }

        public void SetBodyModifier(string bodyTypeDefName, bool side, float newValue)
        {
            if (BodyOffsetsModifiers == null)
                BodyOffsetsModifiers = GetDefaultBodyModifiersDictionary();
            if (BodyOffsetsSideModifiers == null)
                BodyOffsetsSideModifiers = GetDefaultBodyModifiersDictionary();

            bodyTypeDefName = bodyTypeDefName.ToLower();

            Dictionary<string, float> dict = side ? BodyOffsetsSideModifiers : BodyOffsetsModifiers;

            if (dict.ContainsKey(bodyTypeDefName))
            {
                dict[bodyTypeDefName] = newValue;
            }
            else
            {
                dict.Add(bodyTypeDefName, newValue);
            }
        }

        public HolsterConfiguration GetConfiguration(Rot4 rot, bool side)
        {
            if (side)
            {
                if (SideConfiguration.ContainsKey(rot))
                {
                    return SideConfiguration[rot];
                }
            }
            else
            {
                if (Configuration.ContainsKey(rot))
                {
                    return Configuration[rot];
                }
            }

            return HolsterConfiguration.EmptyConfiguration;
        }

        public void Reset()
        {
            Configuration.Clear();
            SideConfiguration.Clear();
            FillWithEmptyEntries();
        }

        public void FillWithEmptyEntries()
        {
            Configuration.Add(Rot4.South, HolsterConfiguration.EmptyConfiguration);
            Configuration.Add(Rot4.North, HolsterConfiguration.EmptyConfiguration);
            Configuration.Add(Rot4.West, HolsterConfiguration.EmptyConfiguration);
            Configuration.Add(Rot4.East, HolsterConfiguration.EmptyConfiguration);

            SideConfiguration.Add(Rot4.South, HolsterConfiguration.EmptyConfiguration);
            SideConfiguration.Add(Rot4.North, HolsterConfiguration.EmptyConfiguration);
            SideConfiguration.Add(Rot4.West, HolsterConfiguration.EmptyConfiguration);
            SideConfiguration.Add(Rot4.East, HolsterConfiguration.EmptyConfiguration);
        }

        public void ExposeData()
        {
            Scribe_Collections.Look(ref Configuration, "configuration", LookMode.Value, LookMode.Deep);
            Scribe_Collections.Look(ref SideConfiguration, "sideConfiguration", LookMode.Value, LookMode.Deep);

            Scribe_Collections.Look(ref BodyOffsetsModifiers, "bodyOffsetsModifiers", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref BodyOffsetsSideModifiers, "bodyOffsetsSideModifiers", LookMode.Value, LookMode.Value);
        }

        private static Dictionary<string, float> GetDefaultBodyModifiersDictionary() => new Dictionary<string, float>
        {
            { "male", 0f },
            { "female", 0f },
            { "fat", 1f },
            { "hulk", 0.5f },
            { "thin", -0.2f },
            { "child", -0.5f }
        };
    }
}
