
namespace HeartSim.classes.DataAndTypes
{
    // Node
    public struct Position
    {
        public double X, Y;
    }

    public enum NodeStateIndex { Rest = 1, ERP = 2, RRP = 3 }

    public enum PathTerminal { Entry = 1, Exit = 2 }

    public enum AVness { AV = 1, NonAV = 2 }

    public struct PathTerminalPair
    {
        public int PathIdx;
        public PathTerminal Terminal;
    }

    public struct NodeParameters
    {
        public string NodeName;
        public NodeStateIndex NodeStateIndex;
        public int TERPCurrent;
        public int TERPDefault;
        public int TRRPCurrent;
        public int TRRPDefault;
        public int TrestCurrent;
        public int TrestDefault;
        public bool Activation;
        public int TerpMin;
        public int TerpMax;
        public int IndexOfPathActivateTheNode;
        public AVness AVness;
        public List<PathTerminalPair> ConnectedPaths;
        public Position NodePos;
    }

    // Path
    public enum PathStateIndex
    {
        Idle = 1,
        AntegradeConduction = 2,
        Retrograde = 3,
        Conflict = 4,
        Double = 5
    }

    public struct PathParameters
    {
        public string PathName;
        public PathStateIndex PathStateIndex;
        public int EntryNodeIndex;
        public int ExitNodeIndex;
        public int AmplitudeFactor;
        public double ForwardSpeed;
        public double BackwardSpeed;
        public double ForwardTimerCurrent;
        public double ForwardTimerDefault;
        public double BackwardTimerCurrent;
        public double BackwardTimerDefault;
        public double PathLength;
        public double PathSlope;
    }

    // EpAvnrtDataGen
    public static class Data
    {
        // Nodes Data
        public static readonly List<string> NodeNames = [
            "SA",
            "CT",
            "AV",
            "OS",
            "His_p",
            "His_m",
            "His_d",
            "Bach",
            "LA_a",
            "LA",
            "RBB_m",
            "RBB",
            "LBB_m",
            "LBB",
            "RVA",
            "LVA",
            "RV_m",
            "RV",
            "LV_m",
            "LV",
            "CT_a",
            "RA_a",
            "RA",
            "SEP_RV_m",
            "SEP_RV",
            "SEP_LV_m",
            "SEP_LV",
            "CS_LV",
            "CS_LA",
            "slow_b",
            "slow_a",
            "fast",
            "fast_b",
        ];

        public static readonly List<List<int>> NodeIntParameters = [
            [1, 220, 220, 10, 20, 700, 700, 0, 150, 300, 0, 2],
            [1, 220, 220, 120, 120, 9999, 9999, 0, 150, 360, 0, 2],
            [1, 320, 320, 250, 250, 9999, 9999, 0, 350, 230, 0, 1],
            [1, 320, 320, 120, 120, 9999, 9999, 0, 100, 200, 0, 2],
            [1, 320, 320, 120, 120, 9999, 9999, 0, 330, 440, 0, 2],
            [1, 320, 320, 120, 120, 9999, 9999, 0, 330, 440, 0, 2],
            [1, 320, 320, 120, 120, 9999, 9999, 0, 330, 440, 0, 2],
            [1, 320, 320, 120, 120, 9999, 9999, 0, 150, 360, 0, 2],
            [1, 320, 320, 120, 120, 9999, 9999, 0, 150, 360, 0, 2],
            [1, 320, 320, 120, 120, 9999, 9999, 0, 150, 360, 0, 2],
            [1, 320, 320, 120, 120, 9999, 9999, 0, 330, 440, 0, 2],
            [1, 320, 320, 120, 120, 9999, 9999, 0, 330, 440, 0, 2],
            [1, 320, 320, 120, 120, 9999, 9999, 0, 330, 440, 0, 2],
            [1, 320, 320, 120, 120, 9999, 9999, 0, 330, 440, 0, 2],
            [1, 320, 320, 120, 120, 9999, 9999, 0, 180, 290, 0, 2],
            [1, 320, 320, 120, 120, 9999, 9999, 0, 180, 290, 0, 2],
            [1, 320, 320, 120, 120, 9999, 9999, 0, 180, 290, 0, 2],
            [1, 320, 320, 120, 120, 9999, 9999, 0, 180, 290, 0, 2],
            [1, 320, 320, 120, 120, 9999, 9999, 0, 180, 290, 0, 2],
            [1, 320, 320, 120, 120, 9999, 9999, 0, 180, 290, 0, 2],
            [1, 220, 220, 120, 120, 9999, 9999, 0, 150, 360, 0, 2],
            [1, 320, 320, 120, 120, 9999, 9999, 0, 150, 360, 0, 2],
            [1, 320, 320, 120, 120, 9999, 9999, 0, 150, 360, 0, 2],
            [1, 320, 320, 120, 120, 9999, 9999, 0, 150, 300, 0, 2],
            [1, 320, 320, 120, 120, 9999, 9999, 0, 150, 300, 0, 2],
            [1, 320, 320, 120, 120, 9999, 9999, 0, 150, 300, 0, 2],
            [1, 320, 320, 120, 120, 9999, 9999, 0, 150, 300, 0, 2],
            [1, 320, 320, 120, 120, 9999, 9999, 0, 150, 300, 0, 2],
            [1, 320, 320, 120, 120, 9999, 9999, 0, 150, 300, 0, 2],
            [1, 320, 320, 200, 200, 9999, 9999, 0, 250, 150, 0, 1],
            [1, 320, 320, 300, 300, 9999, 9999, 0, 250, 150, 0, 1],
            [1, 320, 320, 10, 10, 9999, 9999, 0, 500, 380, 0, 1],
            [1, 320, 320, 10, 10, 9999, 9999, 0, 500, 380, 0, 1],
        ];

        public static readonly List<Position> NodePositions = [
            new Position { X = 135.5, Y = 295.5 },
            new Position { X = 134.5, Y = 161.5 },
            new Position { X = 170.5, Y = 216.5 },
            new Position { X = 165.5, Y = 263.5 },
            new Position { X = 204.5, Y = 248.5 },
            new Position { X = 236.5, Y = 250.5 },
            new Position { X = 262.5, Y = 220.5 },
            new Position { X = 245.5, Y = 344.5 },
            new Position { X = 313.5, Y = 349.5 },
            new Position { X = 338.5, Y = 325.5 },
            new Position { X = 304.5, Y = 161.5 },
            new Position { X = 382.5, Y = 127.5 },
            new Position { X = 319.5, Y = 172.5 },
            new Position { X = 392.5, Y = 149.5 },
            new Position { X = 394.5, Y = 113.5 },
            new Position { X = 410.5, Y = 144.5 },
            new Position { X = 280.5, Y = 96.5 },
            new Position { X = 187.5, Y = 147.5 },
            new Position { X = 398.5, Y = 235.5 },
            new Position { X = 358.5, Y = 303.5 },
            new Position { X = 112.5, Y = 243.5 },
            new Position { X = 175.5, Y = 295.5 },
            new Position { X = 198.5, Y = 269.5 },
            new Position { X = 286.5, Y = 149.5 },
            new Position { X = 207.5, Y = 228.5 },
            new Position { X = 327.5, Y = 189.5 },
            new Position { X = 254.5, Y = 263.5 },
            new Position { X = 312.5, Y = 316.5 },
            new Position { X = 296.5, Y = 334.5 },
            new Position { X = 151.5, Y = 213.5 },
            new Position { X = 150.5, Y = 243.5 },
            new Position { X = 167.5, Y = 243.5 },
            new Position { X = 168.5, Y = 230.5 },
        ];

        // Path Data
        public static readonly List<string> PathNames = [
            "SA_CT_a",
            "CT",
            "slow_AV",
            "SA_OS",
            "fast_AV",
            "SA_Bach",
            "Bach_LA_a",
            "LA",
            "AV_His",
            "His_p",
            "His_d",
            "His_RBB",
            "His_LBB",
            "RBB",
            "LBB",
            "RBB_RV",
            "LBB_LV",
            "RV_LV",
            "RV_m",
            "LV_m",
            "RV",
            "LV",
            "SA_RA_a",
            "RA",
            "SEP_RV",
            "SEP_RV_m",
            "SEP_LV",
            "SEP_LV_m",
            "CS_LV",
            "CS_LA",
            "slow",
            "OS_slow",
            "OS_fast",
            "fast"
        ];

        public static readonly List<List<int>> PathIntParameters = [
            [1, 1, 21, 10],
            [1, 21, 2, 10],
            [1, 30, 3, 5],
            [1, 1, 4, 5],
            [1, 33, 3, 5],
            [1, 1, 8, 5],
            [1, 8, 9, 10],
            [1, 9, 10, 10],
            [1, 3, 5, 2],
            [1, 5, 6, 2],
            [1, 6, 7, 2],
            [1, 7, 11, 2],
            [1, 7, 13, 2],
            [1, 11, 12, 2],
            [1, 13, 14, 2],
            [1, 12, 15, 10],
            [1, 14, 16, 10],
            [1, 15, 16, 10],
            [1, 15, 17, 10],
            [1, 16, 19, 10],
            [1, 17, 18, 10],
            [1, 19, 20, 10],
            [1, 1, 22, 10],
            [1, 22, 23, 10],
            [1, 24, 25, 10],
            [1, 15, 24, 10],
            [1, 26, 27, 10],
            [1, 16, 26, 10],
            [1, 27, 28, 10],
            [1, 23, 29, 5],
            [1, 31, 30, 10],
            [1, 4, 31, 10],
            [1, 4, 32, 10],
            [1, 32, 33, 10],
        ];

        public static readonly List<List<double>> PathDoubleParameters = [
            [1, 1, 57, 57, 57, 57, 56.859475903318, 2.260869565217391],
            [1, 1, 85, 85, 85, 85, 84.8999411071645, -3.727272727272727],
            [1, 1, 19, 19, 19, 19, 19.235384061671343, 1.5277777777777777],
            [1, 1, 44, 44, 44, 44, 43.86342439892262, -1.0666666666666667],
            [1, 1, 47, 47, 47, 47, 14.142135623730951, -7],
            [3.0105024497581794, 3.0105024497581794, 40, 40, 40, 40, 120.42009799032718, 0.44545454545454544],
            [1, 1, 68, 68, 68, 68, 68.18357573492314, 0.07352941176470588],
            [1, 1, 35, 35, 35, 35, 34.655446902326915, -0.96],
            [1, 1, 47, 47, 47, 47, 46.69047011971501, 0.9411764705882353],
            [2, 2, 16, 16, 16, 16, 32.0624390837628, 0.0625],
            [2, 2, 20, 20, 20, 20, 39.698866482558415, -1.1538461538461537],
            [2, 2, 36, 36, 36, 36, 72.42237223399962, -1.4047619047619047],
            [2, 2, 37, 37, 37, 37, 74.51845409024533, -0.8421052631578947],
            [4.25440947723653, 4.25440947723653, 20, 20, 20, 20, 85.0881895447306, -0.4358974358974359],
            [3.8268786236304906, 3.8268786236304906, 20, 20, 20, 20, 76.53757247260981, -0.3150684931506849],
            [1, 1, 18, 18, 18, 18, 18.439088914585774, -1.1666666666666667],
            [1, 1, 19, 19, 19, 19, 18.681541692269406, -0.2777777777777778],
            [2, 2, 17, 17, 17, 17, 34.88552708502482, 1.9375],
            [5.7630287176102115, 5.7630287176102115, 20, 20, 20, 20, 115.26057435220423, 0.14912280701754385],
            [4.589389937671455, 4.589389937671455, 20, 20, 20, 20, 91.7877987534291, -7.583333333333333],
            [5.303300858899107, 5.303300858899107, 20, 20, 20, 20, 106.06601717798213, -0.5483870967741935],
            [3.944616584663204, 3.944616584663204, 20, 20, 20, 20, 78.89233169326408, -1.7],
            [1, 1, 40, 40, 40, 40, 40, 0],
            [1, 1, 35, 35, 35, 35, 34.713109915419565, -1.1304347826086956],
            [2, 2, 56, 56, 56, 56, 111.72287142747452, -1],
            [2, 2, 57, 57, 57, 57, 113.84199576606166, -0.3333333333333333],
            [2, 2, 52, 52, 52, 52, 103.9471019317037, -1.0136986301369864],
            [2, 2, 47, 47, 47, 47, 94.41398201537736, -0.5421686746987951],
            [2, 2, 39, 39, 39, 39, 78.56844150166147, 0.9137931034482759],
            [2, 2, 59, 59, 59, 59, 117.5967686630887, 0.6632653061224489],
            [0.09, 0.09, 330, 330, 330, 330, 30.01666203960727, -30],
            [2, 2, 13, 13, 13, 13, 25, 1.3333333333333333],
            [2, 2, 10, 10, 10, 10, 20.09975124224178, -10],
            [2, 2, 7, 7, 7, 7, 13.038404810405298, -13],
        ];

        // EGM Data
        public static readonly List<string> EgmSingleNames = [
            "HRA1",
            "HRA2",
            "HRA3",
            "HRA4",
            "CS1",
            "CS2",
            "CS3",
            "CS4",
            "CS5",
            "His_d",
            "His_m1",
            "His_m2",
            "His_p",
            "RVA1",
            "RVA2",
            "RVA3",
            "RVA4",
        ];

        public static readonly List<int> EgmSingleIntParameters = [
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            8,
            9,
            10,
            11,
            12,
            13,
            14,
            15,
            16,
            17,
        ];

        public static readonly List<string> EgmDoubleNames = [
            "HRA_d",
            "HRA_m",
            "HRA_p",
            "CS1,2",
            "CS2,3",
            "CS3,4",
            "CS4,5",
            "His_d",
            "His_m",
            "His_p",
            "RVA_d",
            "RVA_m",
            "RVA_p",
        ];

        public static readonly List<Tuple<int, int>> EgmDoubleIntParameters = [
            Tuple.Create(1, 2),
            Tuple.Create(2, 3),
            Tuple.Create(3, 4),
            Tuple.Create(5, 6),
            Tuple.Create(6, 7),
            Tuple.Create(7, 8),
            Tuple.Create(8, 9),
            Tuple.Create(10, 11),
            Tuple.Create(11, 12),
            Tuple.Create(12, 13),
            Tuple.Create(14, 15),
            Tuple.Create(15, 16),
            Tuple.Create(16, 17),
        ];

        // Probe Data
        public static readonly List<string> ProbeNames = [
            "HRA1",
            "HRA2",
            "HRA3",
            "HRA4",
            "CS1",
            "CS2",
            "CS3",
            "CS4",
            "CS5",
            "His_d",
            "His_m1",
            "His_m2",
            "His_p",
            "RVA1",
            "RVA2",
            "RVA3",
            "RVA4",
        ];

        public static readonly List<List<int>> ProbeIntParameters = [
            [23, 24],
            [1, 23, 24],
            [1, 23],
            [1],
            [29, 30],
            [29, 30],
            [29, 30],
            [29, 30],
            [29, 30],
            [24, 25],
            [24, 25],
            [10, 24, 25],
            [24, 25],
            [14, 16, 19, 26],
            [14, 26],
            [14, 26],
            [14, 26],
        ];

        public static readonly List<Position> ProbePositions = [
            new Position { X = 168.5, Y = 280.5 },
            new Position { X = 152.5, Y = 284.5 },
            new Position { X = 136.5, Y = 282.5 },
            new Position { X = 131.5, Y = 266.5 },
            new Position { X = 305.5, Y = 322.5 },
            new Position { X = 289.5, Y = 313.5 },
            new Position { X = 272.5, Y = 305.5 },
            new Position { X = 254.5, Y = 296.5 },
            new Position { X = 237.5, Y = 286.5 },
            new Position { X = 249.5, Y = 227.5 },
            new Position { X = 231.5, Y = 236.5 },
            new Position { X = 213.5, Y = 243.5 },
            new Position { X = 196.5, Y = 249.5 },
            new Position { X = 366.5, Y = 119.5 },
            new Position { X = 351.5, Y = 127.5 },
            new Position { X = 333.5, Y = 137.5 },
            new Position { X = 314.5, Y = 146.5 },
        ];

        public static readonly List<string> ComponentNames = [
            "LRI",
            "AVI",
            "ARP",
            "VRP",
        ];

        public static readonly List<List<int>> PaceIntParameters = [
            [1, 1000, 1000, 0],
            [1, 150, 150, 0],
            [1, 150, 150, 0],
            [1, 320, 320, 0],
        ];
    }
}
