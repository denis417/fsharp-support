﻿using static Module;

public class Class1
{
  public Class1()
  {
    U a = U.CaseA;
    U b = U.CaseB;

    bool isA = a.IsCaseA;
    bool isB = a.IsCaseB;

    int tA = U.Tags.CaseA;
    int tB = U.Tags.CaseB;

    int t = a.Tag;
  }
}
