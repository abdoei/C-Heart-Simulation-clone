//#define Data EpAvnrtDataGen

using HeartSim.classes.DataAndTypes;
using System;
using System.Collections.Generic;


namespace HeartSim.classes.PathNS
{
  public class Path
  {
    private PathParameters _pathParameters;

    public Path(string pathName, List<int> pathIntegerParameters, List<float> pathFloatParameters)
    {
      _pathParameters = new PathParameters
      {
        PathName = pathName,
        PathStateIndex = (PathStateIndexEnum)pathIntegerParameters[0],
        EntryNodeIndex = pathIntegerParameters[1] - 1,
        ExitNodeIndex = pathIntegerParameters[2] - 1,
        AmplitudeFactor = pathIntegerParameters[3],
        ForwardSpeed = pathFloatParameters[0],
        BackwardSpeed = pathFloatParameters[1],
        ForwardTimerCurrent = pathFloatParameters[2],
        ForwardTimerDefault = pathFloatParameters[3],
        BackwardTimerCurrent = pathFloatParameters[4],
        BackwardTimerDefault = pathFloatParameters[5],
        PathLength = pathFloatParameters[6],
        PathSlope = pathFloatParameters[7]
      };
    }

    public (bool, bool) PathAutomaton(NodeTable nt)
    {
      bool tempNode1Activation = false;
      bool tempNode2Activation = false;

      int entryNodeIndex = _pathParameters.EntryNodeIndex;
      int exitNodeIndex = _pathParameters.ExitNodeIndex;

      Node entryNode = nt.NodeTable[entryNodeIndex];
      Node exitNode = nt.NodeTable[exitNodeIndex];

      switch (_pathParameters.PathStateIndex)
      {
        case PathStateIndexEnum.Idle:
          if (entryNode.Parameters.Activation)
            _pathParameters.PathStateIndex = PathStateIndexEnum.AntegradeConduction;
          else if (exitNode.Parameters.Activation)
            _pathParameters.PathStateIndex = PathStateIndexEnum.Retrograde;
          break;
        case PathStateIndexEnum.AntegradeConduction:
          if (exitNode.Parameters.Activation)
            _pathParameters.PathStateIndex = PathStateIndexEnum.Double;
          else
          {
            if (_pathParameters.ForwardTimerCurrent == 0)
            {
              _pathParameters.ForwardTimerCurrent = _pathParameters.ForwardTimerDefault;
              tempNode2Activation = true;
              _pathParameters.PathStateIndex = PathStateIndexEnum.Conflict;
            }
            else
            {
              _pathParameters.ForwardTimerCurrent--;
            }
          }
          break;
        case PathStateIndexEnum.Retrograde:
          if (entryNode.Parameters.Activation)
            _pathParameters.PathStateIndex = PathStateIndexEnum.Double;
          else
          {
            if (_pathParameters.BackwardTimerCurrent == 0)
            {
              _pathParameters.BackwardTimerCurrent = _pathParameters.BackwardTimerDefault;
              tempNode1Activation = true;
              _pathParameters.PathStateIndex = PathStateIndexEnum.Conflict;
            }
            else
            {
              _pathParameters.BackwardTimerCurrent--;
            }
          }
          break;
        case PathStateIndexEnum.Conflict:
          _pathParameters.PathStateIndex = PathStateIndexEnum.Idle;
          break;
        case PathStateIndexEnum.Double:
          if (_pathParameters.BackwardTimerCurrent == 0)
          {
            _pathParameters.BackwardTimerCurrent = _pathParameters.BackwardTimerDefault;
            tempNode1Activation = true;
            _pathParameters.PathStateIndex = PathStateIndexEnum.Conflict;
            return (tempNode1Activation, tempNode2Activation);
          }
          if (_pathParameters.ForwardTimerCurrent == 0)
          {
            _pathParameters.ForwardTimerCurrent = _pathParameters.ForwardTimerDefault;
            tempNode2Activation = true;
            _pathParameters.PathStateIndex = PathStateIndexEnum.Conflict;
            return (tempNode1Activation, tempNode2Activation);
          }
          if (Math.Abs(1 - (_pathParameters.ForwardTimerCurrent / _pathParameters.ForwardTimerDefault) -
                      (_pathParameters.BackwardTimerCurrent / _pathParameters.BackwardTimerDefault)) <
              (0.9 / Math.Min(_pathParameters.ForwardTimerDefault, _pathParameters.BackwardTimerDefault)))
          {
            _pathParameters.BackwardTimerCurrent = _pathParameters.BackwardTimerDefault;
            _pathParameters.ForwardTimerCurrent = _pathParameters.ForwardTimerDefault;
            _pathParameters.PathStateIndex = PathStateIndexEnum.Conflict;
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
    public void SetPathStateIndex(PathStateIndexEnum pathStateIndex) { _pathParameters.PathStateIndex = pathStateIndex; }
    public void SetEntryNodeIndex(int entryNodeIndex) { _pathParameters.EntryNodeIndex = entryNodeIndex; }
    public void SetExitNodeIndex(int exitNodeIndex) { _pathParameters.ExitNodeIndex = exitNodeIndex; }
    public void SetAmplitudeFactor(int amplitudeFactor) { _pathParameters.AmplitudeFactor = amplitudeFactor; }
    public void SetForwardSpeed(float forwardSpeed) { _pathParameters.ForwardSpeed = forwardSpeed; }
    public void SetBackwardSpeed(float backwardSpeed) { _pathParameters.BackwardSpeed = backwardSpeed; }
    public void SetForwardTimerCurrent(float forwardTimerCurrent) { _pathParameters.ForwardTimerCurrent = forwardTimerCurrent; }
    public void SetForwardTimerDefault(float forwardTimerDefault) { _pathParameters.ForwardTimerDefault = forwardTimerDefault; }
    public void SetBackwardTimerCurrent(float backwardTimerCurrent) { _pathParameters.BackwardTimerCurrent = backwardTimerCurrent; }
    public void SetBackwardTimerDefault(float backwardTimerDefault) { _pathParameters.BackwardTimerDefault = backwardTimerDefault; }
    public void SetPathLength(float pathLength) { _pathParameters.PathLength = pathLength; }
    public void SetPathSlope(float pathSlope) { _pathParameters.PathSlope = pathSlope; }
  }


  public class PathTable
  {
    public List<List<PathTerminalPair>> PathTerminalPairsPerPointList;
    public List<Path> path_table;

    public PathTable(List<string> pathNames, List<List<int>> pathIntegerParameters, List<List<float>> pathFloatParameters)
    {
      PathTerminalPairsPerPointList = new List<List<PathTerminalPair>>();
      path_table = new List<Path>();

      for (int i = 0; i < pathNames.Count; i++)
      {
        path_table.Add(new Path(pathNames[i], pathIntegerParameters[i], pathFloatParameters[i]));
      }

      Dictionary<int, List<PathTerminalPair>> nodeIdxPathsTerminalsMap = new Dictionary<int, List<PathTerminalPair>>();

      for (int pathIdx = 0; pathIdx < path_table.Count; pathIdx++)
      {
        int entryIdx = path_table[pathIdx].GetParameters().EntryNodeIndex;
        int exitIdx = path_table[pathIdx].GetParameters().ExitNodeIndex;
        if (!nodeIdxPathsTerminalsMap.ContainsKey(entryIdx))
          nodeIdxPathsTerminalsMap[entryIdx] = new List<PathTerminalPair>();
        nodeIdxPathsTerminalsMap[entryIdx].Add(new PathTerminalPair { PathIdx = pathIdx, Terminal = PathTerminalEnum.Entry });
        if (!nodeIdxPathsTerminalsMap.ContainsKey(exitIdx))
          nodeIdxPathsTerminalsMap[exitIdx] = new List<PathTerminalPair>();
        nodeIdxPathsTerminalsMap[exitIdx].Add(new PathTerminalPair { PathIdx = pathIdx, Terminal = PathTerminalEnum.Exit });
      }

      int nodeCount = Data.NodeNames.Count;
      for (int nodeIdx = 0; nodeIdx < nodeCount; nodeIdx++)
      {
        PathTerminalPairsPerPointList.Add(nodeIdxPathsTerminalsMap.ContainsKey(nodeIdx) ? nodeIdxPathsTerminalsMap[nodeIdx] : new List<PathTerminalPair>());
      }
    }
  }
}
