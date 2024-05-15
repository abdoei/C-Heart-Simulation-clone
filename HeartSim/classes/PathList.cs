using HeartSim.classes.DataAndTypes;
using HeartSim.classes.NodeNS;
using System;
using System.Collections.Generic;

namespace HeartSim.classes.PathNS
{
  public class Path
  {
    private PathParameters _pathParameters;

    public Path(string pathName, List<int> pathIntegerParameters, List<double> pathdoubleParameters)
    {
      _pathParameters = new PathParameters
      {
        PathName = pathName,
        PathStateIndex = (PathStateIndex)pathIntegerParameters[0],
        EntryNodeIndex = pathIntegerParameters[1] - 1,
        ExitNodeIndex = pathIntegerParameters[2] - 1,
        AmplitudeFactor = pathIntegerParameters[3],
        ForwardSpeed = pathdoubleParameters[0],
        BackwardSpeed = pathdoubleParameters[1],
        ForwardTimerCurrent = pathdoubleParameters[2],
        ForwardTimerDefault = pathdoubleParameters[3],
        BackwardTimerCurrent = pathdoubleParameters[4],
        BackwardTimerDefault = pathdoubleParameters[5],
        PathLength = pathdoubleParameters[6],
        PathSlope = pathdoubleParameters[7]
      };
    }

    // Copy constructor for deep copy
    public Path(Path path)
    {
      _pathParameters = new PathParameters
      {
        PathName = path._pathParameters.PathName,
        PathStateIndex = path._pathParameters.PathStateIndex,
        EntryNodeIndex = path._pathParameters.EntryNodeIndex,
        ExitNodeIndex = path._pathParameters.ExitNodeIndex,
        AmplitudeFactor = path._pathParameters.AmplitudeFactor,
        ForwardSpeed = path._pathParameters.ForwardSpeed,
        BackwardSpeed = path._pathParameters.BackwardSpeed,
        ForwardTimerCurrent = path._pathParameters.ForwardTimerCurrent,
        ForwardTimerDefault = path._pathParameters.ForwardTimerDefault,
        BackwardTimerCurrent = path._pathParameters.BackwardTimerCurrent,
        BackwardTimerDefault = path._pathParameters.BackwardTimerDefault,
        PathLength = path._pathParameters.PathLength,
        PathSlope = path._pathParameters.PathSlope
      };
    }

    public (bool, bool) PathAutomaton(ref NodeTable nt)
    {
      bool tempNode1Activation = false;
      bool tempNode2Activation = false;

      int entryNodeIndex = _pathParameters.EntryNodeIndex;
      int exitNodeIndex = _pathParameters.ExitNodeIndex;

      Node entryNode = nt.node_table[entryNodeIndex];
      //entryNode = nt.
      Node exitNode = nt.node_table[exitNodeIndex];

      switch (_pathParameters.PathStateIndex)
      {
        case PathStateIndex.Idle:
          if (entryNode.GetParameters().Activation)
            _pathParameters.PathStateIndex = PathStateIndex.AntegradeConduction;
          else if (exitNode.GetParameters().Activation)
            _pathParameters.PathStateIndex = PathStateIndex.Retrograde;
          break;
        case PathStateIndex.AntegradeConduction:
          if (exitNode.GetParameters().Activation)
            _pathParameters.PathStateIndex = PathStateIndex.Double;
          else
          {
            if (_pathParameters.ForwardTimerCurrent == 0)
            {
              _pathParameters.ForwardTimerCurrent = _pathParameters.ForwardTimerDefault;
              tempNode2Activation = true;
              _pathParameters.PathStateIndex = PathStateIndex.Conflict;
            }
            else
            {
              _pathParameters.ForwardTimerCurrent--;
            }
          }
          break;
        case PathStateIndex.Retrograde:
          if (entryNode.GetParameters().Activation)
            _pathParameters.PathStateIndex = PathStateIndex.Double;
          else
          {
            if (_pathParameters.BackwardTimerCurrent == 0)
            {
              _pathParameters.BackwardTimerCurrent = _pathParameters.BackwardTimerDefault;
              tempNode1Activation = true;
              _pathParameters.PathStateIndex = PathStateIndex.Conflict;
            }
            else
            {
              _pathParameters.BackwardTimerCurrent--;
            }
          }
          break;
        case PathStateIndex.Conflict:
          _pathParameters.PathStateIndex = PathStateIndex.Idle;
          break;
        case PathStateIndex.Double:
          if (_pathParameters.BackwardTimerCurrent == 0)
          {
            _pathParameters.BackwardTimerCurrent = _pathParameters.BackwardTimerDefault;
            tempNode1Activation = true;
            _pathParameters.PathStateIndex = PathStateIndex.Conflict;
            return (tempNode1Activation, tempNode2Activation);
          }
          if (_pathParameters.ForwardTimerCurrent == 0)
          {
            _pathParameters.ForwardTimerCurrent = _pathParameters.ForwardTimerDefault;
            tempNode2Activation = true;
            _pathParameters.PathStateIndex = PathStateIndex.Conflict;
            return (tempNode1Activation, tempNode2Activation);
          }
          if (Math.Abs(1 - (_pathParameters.ForwardTimerCurrent / _pathParameters.ForwardTimerDefault) -
                      (_pathParameters.BackwardTimerCurrent / _pathParameters.BackwardTimerDefault)) <
              (0.9 / Math.Min(_pathParameters.ForwardTimerDefault, _pathParameters.BackwardTimerDefault)))
          {
            _pathParameters.BackwardTimerCurrent = _pathParameters.BackwardTimerDefault;
            _pathParameters.ForwardTimerCurrent = _pathParameters.ForwardTimerDefault;
            _pathParameters.PathStateIndex = PathStateIndex.Conflict;
          }
          else
          {
            _pathParameters.ForwardTimerCurrent--;
            _pathParameters.BackwardTimerCurrent--;
          }
          break;
      }

      return (tempNode1Activation, tempNode2Activation);
    }

    public PathParameters GetParameters() { return _pathParameters; }
    public void SetParameters(PathParameters parameters) { _pathParameters = parameters; }
    // setters 
    public void SetPathName(string pathName) { _pathParameters.PathName = pathName; }
    public void SetPathStateIndex(PathStateIndex pathStateIndex) { _pathParameters.PathStateIndex = pathStateIndex; }
    public void SetEntryNodeIndex(int entryNodeIndex) { _pathParameters.EntryNodeIndex = entryNodeIndex; }
    public void SetExitNodeIndex(int exitNodeIndex) { _pathParameters.ExitNodeIndex = exitNodeIndex; }
    public void SetAmplitudeFactor(int amplitudeFactor) { _pathParameters.AmplitudeFactor = amplitudeFactor; }
    public void SetForwardSpeed(double forwardSpeed) { _pathParameters.ForwardSpeed = forwardSpeed; }
    public void SetBackwardSpeed(double backwardSpeed) { _pathParameters.BackwardSpeed = backwardSpeed; }
    public void SetForwardTimerCurrent(double forwardTimerCurrent) { _pathParameters.ForwardTimerCurrent = forwardTimerCurrent; }
    public void SetForwardTimerDefault(double forwardTimerDefault) { _pathParameters.ForwardTimerDefault = forwardTimerDefault; }
    public void SetBackwardTimerCurrent(double backwardTimerCurrent) { _pathParameters.BackwardTimerCurrent = backwardTimerCurrent; }
    public void SetBackwardTimerDefault(double backwardTimerDefault) { _pathParameters.BackwardTimerDefault = backwardTimerDefault; }
    public void SetPathLength(double pathLength) { _pathParameters.PathLength = pathLength; }
    public void SetPathSlope(double pathSlope) { _pathParameters.PathSlope = pathSlope; }
  }


  public class PathTable
  {
    public List<List<PathTerminalPair>> PathTerminalPairsPerPointList;
    public List<Path> path_table;

    public PathTable(List<string> pathNames, List<List<int>> pathIntegerParameters, List<List<double>> pathdoubleParameters)
    {
      PathTerminalPairsPerPointList = new List<List<PathTerminalPair>>();
      path_table = new List<Path>();

      for (int i = 0; i < pathNames.Count; i++)
      {
        path_table.Add(new Path(pathNames[i], pathIntegerParameters[i], pathdoubleParameters[i]));
      }

      Dictionary<int, List<PathTerminalPair>> nodeIdxPathsTerminalsMap = new Dictionary<int, List<PathTerminalPair>>();
      for (int pathIdx = 0; pathIdx < path_table.Count; pathIdx++)
      {
        int entryIdx = path_table[pathIdx].GetParameters().EntryNodeIndex;
        int exitIdx = path_table[pathIdx].GetParameters().ExitNodeIndex;
        if (!nodeIdxPathsTerminalsMap.ContainsKey(entryIdx))
          nodeIdxPathsTerminalsMap[entryIdx] = new List<PathTerminalPair>();
        nodeIdxPathsTerminalsMap[entryIdx].Add(new PathTerminalPair { PathIdx = pathIdx, Terminal = PathTerminal.Entry });
        if (!nodeIdxPathsTerminalsMap.ContainsKey(exitIdx))
          nodeIdxPathsTerminalsMap[exitIdx] = new List<PathTerminalPair>();
        nodeIdxPathsTerminalsMap[exitIdx].Add(new PathTerminalPair { PathIdx = pathIdx, Terminal = PathTerminal.Exit });
      }

      int nodeCount = Data.NodeNames.Count;
      for (int nodeIdx = 0; nodeIdx < nodeCount; nodeIdx++)
      {
        PathTerminalPairsPerPointList.Add(nodeIdxPathsTerminalsMap.ContainsKey(nodeIdx) ? nodeIdxPathsTerminalsMap[nodeIdx] : new List<PathTerminalPair>());
      }
    }

    // copy constructor for deep copy
    public PathTable(PathTable pathTable)
    {
      PathTerminalPairsPerPointList = new List<List<PathTerminalPair>>();
      path_table = new List<Path>();

      foreach (Path path in pathTable.path_table)
      {
        path_table.Add(new Path(path));
      }

      foreach (List<PathTerminalPair> pathTerminalPairs in pathTable.PathTerminalPairsPerPointList)
      {
        PathTerminalPairsPerPointList.Add(new List<PathTerminalPair>(pathTerminalPairs));
      }
    }

  }
}
