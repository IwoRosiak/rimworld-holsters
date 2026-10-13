using RimWorldHolsters.Utility.Old;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorldHolsters
{
    public struct WeaponGroupCordInfo : IExposable
    {
        public WeaponGroupCordInfo(string name)
        {
            this.name = name;
            weapons = new List<string>();
            isDisplay = true;

            pos = new Dictionary<Rot4, Vector3>()
            {
                {Rot4.South, Vector3.zero},
                {Rot4.North, Vector3.zero},
                {Rot4.East, Vector3.zero},
                {Rot4.West, Vector3.zero}
            };

            flip = new Dictionary<Rot4, bool>()
            {
                {Rot4.South, true},
                {Rot4.North, false},
                {Rot4.East, false},
                {Rot4.West, true}
            };

            angle = new Dictionary<Rot4, float>()
            {
                {Rot4.South, 0},
                {Rot4.North, 0},
                {Rot4.East, 0},
                {Rot4.West, 0}
            };

            posSide = new Dictionary<Rot4, Vector3>()
            {
                {Rot4.South, Vector3.zero},
                {Rot4.North, Vector3.zero},
                {Rot4.East, Vector3.zero},
                {Rot4.West, Vector3.zero}
            };

            flipSide = new Dictionary<Rot4, bool>()
            {
                {Rot4.South, false},
                {Rot4.North, true},
                {Rot4.East, false},
                {Rot4.West, true}
            };

            angleSide = new Dictionary<Rot4, float>()
            {
                {Rot4.South, 0},
                {Rot4.North, 0},
                {Rot4.East, 0},
                {Rot4.West, 0}
            };
            bodyOffset = new Dictionary<Rot4, Vector3>()
            {
                {Rot4.South, Vector3.zero},
                {Rot4.North, Vector3.zero},
                {Rot4.East, Vector3.zero},
                {Rot4.West, Vector3.zero}
            };

            bodyOffsetSide = new Dictionary<Rot4, Vector3>()
            {
                {Rot4.South, Vector3.zero},
                {Rot4.North, Vector3.zero},
                {Rot4.East, Vector3.zero},
                {Rot4.West, Vector3.zero}
            };

            bodyOffsetModifs = new Dictionary<BodyType, float>()
            {
                {BodyType.male, 0},
                {BodyType.female, 0},
                {BodyType.fat, 1},
                {BodyType.hulk, 0.5f},
                {BodyType.thin, -0.2f}
            };

            bodyOffsetModifsSide = new Dictionary<BodyType, float>()
            {
                {BodyType.male, 0},
                {BodyType.female, 0},
                {BodyType.fat, 1},
                {BodyType.hulk, 0.5f},
                {BodyType.thin, -0.2f}
            };

            size = new Dictionary<Rot4, float>()
            {
                {Rot4.South, 1},
                {Rot4.North, 1},
                {Rot4.East, 1},
                {Rot4.West, 1}
            };

            sizeSide = new Dictionary<Rot4, float>()
            {
                {Rot4.South, 1},
                {Rot4.North, 1},
                {Rot4.East, 1},
                {Rot4.West, 1}
            };


            isFront = new Dictionary<Rot4, bool>()
            {
                {Rot4.South, true},
                {Rot4.North, false},
                {Rot4.East, false},
                {Rot4.West, true}
            };

            isFrontSide = new Dictionary<Rot4, bool>()
            {
                {Rot4.South, true},
                {Rot4.North, false},
                {Rot4.East, false},
                {Rot4.West, true}
            };
        }

        public WeaponGroupCordInfo(string name, Dictionary<Rot4, Vector3> pos, Dictionary<Rot4, float> angle, Dictionary<Rot4, bool> flip, Dictionary<Rot4, Vector3> posSide, Dictionary<Rot4, float> angleSide, Dictionary<Rot4, bool> flipSide, Dictionary<Rot4, float> size, Dictionary<Rot4, bool> isFront, Dictionary<Rot4, bool> isFrontSide, Dictionary<Rot4, Vector3> bodyOffset, Dictionary<Rot4, Vector3> bodyOffsetSide, Dictionary<BodyType, float> bodyOffsetModifs)
        {
            this.name = name;
            weapons = new List<string>();
            isDisplay = true;

            this.pos = pos;
            this.angle = angle;
            this.flip = flip;
            this.posSide = posSide;
            this.angleSide = angleSide;
            this.flipSide = flipSide;
            this.isFront = isFront;

            this.isFrontSide = isFrontSide;

            this.bodyOffset = bodyOffset;
            this.bodyOffsetSide = bodyOffsetSide;

            this.bodyOffsetModifs = bodyOffsetModifs;

            bodyOffsetModifsSide = bodyOffsetModifs;

            this.size = size;

            sizeSide = size;
        }

        public void Reset()
        {
            pos = new Dictionary<Rot4, Vector3>()
            {
                {Rot4.South, Vector3.zero},
                {Rot4.North, Vector3.zero},
                {Rot4.East, Vector3.zero},
                {Rot4.West, Vector3.zero}
            };

            flip = new Dictionary<Rot4, bool>()
            {
                {Rot4.South, true},
                {Rot4.North, false},
                {Rot4.East, false},
                {Rot4.West, true}
            };

            angle = new Dictionary<Rot4, float>()
            {
                {Rot4.South, 0},
                {Rot4.North, 0},
                {Rot4.East, 0},
                {Rot4.West, 0}
            };

            bodyOffset = new Dictionary<Rot4, Vector3>()
            {
                {Rot4.South, Vector3.zero},
                {Rot4.North, Vector3.zero},
                {Rot4.East, Vector3.zero},
                {Rot4.West, Vector3.zero}
            };

            posSide = new Dictionary<Rot4, Vector3>()
            {
                {Rot4.South, Vector3.zero},
                {Rot4.North, Vector3.zero},
                {Rot4.East, Vector3.zero},
                {Rot4.West, Vector3.zero}
            };

            flipSide = new Dictionary<Rot4, bool>()
            {
                {Rot4.South, false},
                {Rot4.North, true},
                {Rot4.East, false},
                {Rot4.West, true}
            };

            angleSide = new Dictionary<Rot4, float>()
            {
                {Rot4.South, 0},
                {Rot4.North, 0},
                {Rot4.East, 0},
                {Rot4.West, 0}
            };

            bodyOffsetSide = new Dictionary<Rot4, Vector3>()
            {
                {Rot4.South, Vector3.zero},
                {Rot4.North, Vector3.zero},
                {Rot4.East, Vector3.zero},
                {Rot4.West, Vector3.zero}
            };

            bodyOffsetModifs = new Dictionary<BodyType, float>()
            {
                {BodyType.male, 0},
                {BodyType.female, 0},
                {BodyType.fat, 1},
                {BodyType.hulk, 0.5f},
                {BodyType.thin, -0.2f}
            };

            bodyOffsetModifsSide = new Dictionary<BodyType, float>()
            {
                {BodyType.male, 0},
                {BodyType.female, 0},
                {BodyType.fat, 1},
                {BodyType.hulk, 0.5f},
                {BodyType.thin, -0.2f}
            };

            size = new Dictionary<Rot4, float>()
            {
                {Rot4.South, 1},
                {Rot4.North, 1},
                {Rot4.East, 1},
                {Rot4.West, 1}
            };

            sizeSide = new Dictionary<Rot4, float>()
            {
                {Rot4.South, 1},
                {Rot4.North, 1},
                {Rot4.East, 1},
                {Rot4.West, 1}
            };

            isFront = new Dictionary<Rot4, bool>()
            {
                {Rot4.South, true},
                {Rot4.North, false},
                {Rot4.East, false},
                {Rot4.West, true}
            };

            isFrontSide = new Dictionary<Rot4, bool>()
            {
                {Rot4.South, true},
                {Rot4.North, false},
                {Rot4.East, false},
                {Rot4.West, true}
            };

        }

        private string name;

        public List<string> weapons;

        public bool isDisplay;

        public Dictionary<Rot4, Vector3> pos;
        public Dictionary<Rot4, float> angle;
        public Dictionary<Rot4, bool> flip;
        public Dictionary<Rot4, bool> isFront;

        public Dictionary<Rot4, Vector3> posSide;
        public Dictionary<Rot4, float> angleSide;
        public Dictionary<Rot4, bool> flipSide;
        public Dictionary<Rot4, bool> isFrontSide;

        public Dictionary<BodyType, float> bodyOffsetModifs;
        public Dictionary<BodyType, float> bodyOffsetModifsSide;

        public Dictionary<Rot4, Vector3> bodyOffset;
        public Dictionary<Rot4, Vector3> bodyOffsetSide;

        private Dictionary<Rot4, float> size;
        private Dictionary<Rot4, float> sizeSide;

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public Vector3 GetPos(Rot4 rot, bool isSidearm)
        {
            if (isSidearm)
            {
                if (!posSide.NullOrEmpty() && posSide?.ContainsKey(rot) == true)
                {
                    return posSide[rot] / 10;
                }
            }
            else if (!pos.NullOrEmpty() && pos?.ContainsKey(rot) == true)
            {
                return pos[rot] / 10;
            }

            return Vector3.zero;
        }

        public float GetAngle(Rot4 rot, bool isSidearm)
        {
            if (isSidearm)
            {
                if (!angleSide.NullOrEmpty() && angleSide?.ContainsKey(rot) == true)
                {
                    return angleSide[rot];
                }
            }
            else if (!angle.NullOrEmpty() && angle?.ContainsKey(rot) == true)
            {
                return angle[rot];
            }

            return 0;
        }

        public bool GetFlip(Rot4 rot, bool isSidearm)
        {
            if (isSidearm)
            {
                if (!flipSide.NullOrEmpty() && flipSide?.ContainsKey(rot) == true)
                {
                    return flipSide[rot];
                }
            }
            else
            {
                if (!flip.NullOrEmpty() && flip?.ContainsKey(rot) == true)
                {
                    return flip[rot];
                }
            }

            return false;
        }

        public Vector3 GetBodyOffset(Rot4 rot, bool isSidearm)
        {
            if (isSidearm)
            {
                if (!bodyOffsetSide.NullOrEmpty() && bodyOffsetSide?.ContainsKey(rot) == true)
                {
                    return bodyOffsetSide[rot] / 10;
                }
            }
            else if (!bodyOffset.NullOrEmpty() && bodyOffset?.ContainsKey(rot) == true)
            {
                return bodyOffset[rot] / 10;
            }

            return Vector3.zero;
        }

        public float GetBodyOffsetModif(BodyType bodyType, bool isSidearm)
        {
            if (isSidearm)
            {
                if (!bodyOffsetModifsSide.NullOrEmpty() && bodyOffsetModifsSide?.ContainsKey(bodyType) == true)
                {
                    return bodyOffsetModifsSide[bodyType];
                }
            }
            else if (!bodyOffsetModifs.NullOrEmpty() && bodyOffsetModifs?.ContainsKey(bodyType) == true)
            {
                return bodyOffsetModifs[bodyType];
            }

            return 0;
        }

        public float GetSize(Rot4 dir)
        {
            if (!size.NullOrEmpty() && size?.ContainsKey(dir) == true)
            {
                return size[dir];
            }

            return 1;
        }

        public bool GetLayer(Rot4 dir, bool isSidearm)
        {
            if (isSidearm)
            {
                if (!isFrontSide.NullOrEmpty() && isFrontSide?.ContainsKey(dir) == true)
                {
                    return isFrontSide[dir];
                }
            }
            else if (!isFront.NullOrEmpty() && isFront?.ContainsKey(dir) == true)
            {
                return isFront[dir];
            }

            return false;
        }


        public void SetSize(Rot4 dir, float val)
        {
            size[dir] = val;
        }

        public void SetPos(Rot4 rot, Vector3 val, bool isSidearm)
        {
            if (isSidearm)
            {
                posSide[rot] = val * 10;
            }
            else
            {
                pos[rot] = val * 10;
            }
        }

        public void SetAngle(Rot4 rot, float angle, bool isSidearm)
        {
            if (isSidearm)
            {
                angleSide[rot] = angle;
            }
            else
            {
                this.angle[rot] = angle;
            }
        }

        public void SetFlip(Rot4 rot, bool flip, bool isSidearm)
        {
            if (isSidearm)
            {
                flipSide[rot] = flip;
            }
            else
            {
                this.flip[rot] = flip;
            }
        }

        public void SetBodyOffset(Rot4 rot, Vector3 val, bool isSidearm)
        {
            if (isSidearm)
            {
                bodyOffsetSide[rot] = val * 10;
            }
            else
            {
                bodyOffset[rot] = val * 10;
            }
        }

        public void SetBodyOffsetModif(BodyType bodyType, float val, bool isSidearm)
        {
            if (isSidearm)
            {
                bodyOffsetModifsSide[bodyType] = val;
            }
            else
            {
                bodyOffsetModifs[bodyType] = val;
            }
        }

        public void SetLayer(Rot4 rot, bool val, bool isSidearm)
        {
            if (isSidearm)
            {
                isFrontSide[rot] = val;
            }
            else
            {
                isFront[rot] = val;
            }
        }

        public void ExposeData()
        {
            Scribe_Values.Look(ref isDisplay, "isDisplay", true);
            Scribe_Values.Look(ref name, "name");

            Scribe_Collections.Look(ref pos, "pos", LookMode.Value);
            Scribe_Collections.Look(ref angle, "angle", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref flip, "flip", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref bodyOffsetModifs, "bodyOffsetModifs", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref bodyOffset, "bodyOffset", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref size, "size", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref isFront, "isFront", LookMode.Value, LookMode.Value);

            Scribe_Collections.Look(ref posSide, "posSide", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref angleSide, "angleSide", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref flipSide, "flipSide", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref bodyOffsetModifsSide, "bodyOffsetModifsSide", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref bodyOffsetSide, "bodyOffsetSide", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref isFrontSide, "isFrontSide", LookMode.Value, LookMode.Value);

            Scribe_Collections.Look(ref weapons, "weapons", LookMode.Value);


        }
    }
}
