#include<iostream>
#include<fstream>
#include<sstream>
#include<cmath>
#include<algorithm>
#include<cstdlib>
#include<queue>
#include<map>
#include<string>
#include<stack>
#include<vector>
#include<set>
#include<iomanip>
#include<stdio.h>
#include <math.h>
#define int long long
using namespace std;

int t[18];
int hp[18];
int mytype[6];
int notype[6];
int mcount = 0;
int ncount = 0;
double prior[9];
int num;
double statt[] = { 0,0,0,0 };
int dist[] = { 0,0,0,8,10,12 };
string ans = "";
void action(int x, int pos, string act)
{
	if (abs(x - pos) < 2)
	{
		if (x > pos)
		{
			ans = "R  " + act;
		}
		if (x < pos)
		{
			ans = "L  " + act;
		}
		if (x == pos)
		{
			ans = "S  " + act;
		}
	}
	else
	{
		if (x > pos)
		{
			ans = "R  ";
		}
		if (x < pos)
		{
			ans = "L  ";
		}
		ans += "0";
	}
}
int currentbuildplace = 0;
int hp1, hp2, g1, g2, pos1, pos2;
int obstation = -1;
void fire()
{
	if (obstation == -1)
	{
		action(currentbuildplace, pos1, "2");
	}
	else
	{
		if (abs(obstation - pos1) <= 1)
		{
			if (g1 < 200)
			{
				action(obstation, pos1, "0");
			}
			else
			{
				action(obstation, pos1, "6");
			}
		}
		else
		{
			if (abs(obstation - pos1) <= 4)
				action(obstation, pos1, "0");
			else
			{
				action(currentbuildplace, pos1, "2");
			}
		}
	}
}

void main()
{
	//#if defined(_DEBUG)
	ifstream cin("input.txt");
	ofstream cout("output.txt");
	//#endif
	cin >> num;
	cin >> hp1 >> g1 >> pos1;
	cin >> hp2 >> g2 >> pos2;

	for (int i = 0; i < 18; i++)
	{
		cin >> t[i];
		if (t[i] != 0)
		{
			if (i < 9)
			{
				mcount++;
				mytype[t[i]]++;
				if (t[i] == 2)
				{
					if (obstation == -1)
						obstation = i;
					else
					{
						if (abs(pos1 - obstation) > abs(pos1 - i))
						{
							obstation = i;
						}
					}
				}
				if (t[i]<2)
					prior[i] = 10000;
				else
				{
					prior[i] = 100000;
				}
			}
			else
			{
				ncount++;
				int in = i + 1 - dist[t[i]];
				if (in >= 0 && in<9)
				{
					prior[in]++;
				}
				if (in + 1 < 9)
				{
					prior[in] += 1 / 2;
				}
				if (in - 1 >= 0)
				{
					prior[in] += 1 / 2;
				}
				notype[t[i]]++;
			}
		}
	}
	int ark = mytype[3] + mytype[4] + mytype[5];
	for (int i = 0; i < 18; i++)
	{
		cin >> hp[i];
	}
	for (int i = 0; i < 9; i++)
	{
		prior[i] += abs(pos1 - i);
	}

	int min = prior[0];
	for (int i = 0; i < 9; i++)
	{
		if (prior[i] < min)
		{
			min = prior[i];
			currentbuildplace = i;
		}
	}
	int del = 0;
	if (abs(pos1 - obstation) <= 1)
	{
		if (pos1 == obstation)
		{
			del = 2;
		}
		else
		{
			del = 1;
		}
	}
	statt[3] = ncount - mcount / 2 + num / 40;
	statt[0] = 8 - mytype[1] - num / 40 - del;
	if (mytype[1] < 2 && num<175)
	{
		statt[0] = 100;
	}
	if (g1 > 3000)
	{
		statt[0] = 0;
	}

	statt[1] = 8 - hp1 / 1000 + num / 40;
	statt[2] = (hp1 - hp2) / 1000 + num / 18;
	if (mytype[5] + mytype[4] < 2 && num>150)
	{
		statt[2] += 10;
	}
	int maxx = 0;
	for (int i = 0; i < 4; i++)
	{
		if (statt[maxx] < statt[i])
		{
			maxx = i;
		}
	}
	double towerp = 0;
	double buildp = 0;
	for (int i = 0; i < 9; i++)
	{
		if (t[i] > 2)
		{
			if (dist[t[i]] + i < 9)
			{
				buildp--;
			}
			if (dist[t[i]] + i > 17)
			{
				towerp++;
			}
			if (dist[t[i]] + i + 1 > 17)
			{
				towerp += 1 / 2;
			}
			if (dist[t[i]] + i + 1 > 17)
			{
				towerp += 1 / 2;
			}
			if (dist[t[i]] + i - 1 > 17)
			{
				towerp += 1 / 2;
			}
			if (dist[t[i]] + i >= 9 && dist[t[i]] + i <= 17)
			{
				if (t[dist[t[i]] + i])
				{
					buildp++;
				}
			}
			if (dist[t[i]] + i - 1 >= 9 && dist[t[i]] + i - 1 <= 17)
			{
				if (t[dist[t[i]] + i - 1])
				{
					buildp += 1 / 2;
				}
			}
			if (dist[t[i]] + i + 1 >= 9 && dist[t[i]] + i + 1 <= 17)
			{
				if (t[dist[t[i]] + i + 1])
				{
					buildp += 1 / 2;
				}
			}
		}
	}
	if (maxx == 0)
	{
		if (prior[currentbuildplace]<1000)
			action(currentbuildplace, pos1, "1");
		else
		{
			if (ark<3)
				action(currentbuildplace, pos1, "3");
			else
			{
				if (buildp + towerp > 2)
				{
					fire();
				}
				else
				{
					if (obstation == -1)
						action(currentbuildplace, pos1, "2");
					else
						action(obstation, pos1, "0");
				}
			}
		}
	}
	else
	{
		if (maxx == 1)
		{
			int d = 0;
			for (int i = 0; i < 3; i++)
			{
				if (prior[d] > prior[i])
				{
					d = i;
				}
			}
			if (ark<4 && (ark<3 || prior[d]>1000))
				action(d, pos1, "4");
			else
			{
				if (buildp + towerp > 2)
				{
					fire();
				}
				else
				{
					if (obstation == -1)
						action(currentbuildplace, pos1, "2");
					else
						if (g1 > 2000)
						{
							action(currentbuildplace, pos1, "4");
						}
						else
						{
							fire();
						}
				}
			}
		}
		else
		{
			if (maxx == 2)
			{
				int d = 6;
				if (t[d] == 5 || t[d] == 4)
				{
					d++;
				}
				int ff = 0;
				for (int i = 6; i < 9; i++)
				{
					if (prior[d] > prior[i] && t[i] != 5 && t[i] != 4)
					{
						d = i;
					}
					if (t[i] > 3)
					{
						ff++;
					}
				}
				if (ff < 2)
				{
					action(d, pos1, "5");
				}
				else
				{
					if (towerp > 1.5)
					{
						fire();
					}
					else
					{
						action(d, pos1, "5");
					}
				}
			}
			else
			{
				if (ark <4 && (ark < 3 || prior[currentbuildplace]>1000))
				{
					if (g1 > 2500)
					{
						action(currentbuildplace, pos1, "4");
					}
					else
					{
						if (num > 100)
						{
							action(currentbuildplace, pos1, "5");
						}
						else
						{
							action(currentbuildplace, pos1, "3");
						}
					}
				}
				else
				{
					if (buildp < 2)
					{
						int d = 4;
						int ff = 0;
						for (int i = 4; i < 7; i++)
						{
							if (prior[d] > prior[i])
							{
								d = i;
							}
							if (t[i] > 2)
							{
								ff++;
							}
						}
						if (ff < 2)
						{
							action(d, pos1, "4");
						}
						else
						{
							if (prior[currentbuildplace]<1000)
								action(currentbuildplace, pos1, "1");
							else
							{
								if (towerp > 2)
								{
									fire();
								}
								else
								{
									if (g1 > 2000)
									{
										action(currentbuildplace, pos1, "4");
									}
									else
									{
										fire();
									}
								}
							}
						}
					}
					else
					{
						fire();
					}
				}
			}
		}
	}
	cout << ans;
}