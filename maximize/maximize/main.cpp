#include <iostream>
#include <cmath>
#include <bitset>
#include <vector>
#include <algorithm>
#include <cassert>


using namespace std;

typedef bitset<5> solution_t;


double sex_probability = 0.75;
double mutation_probability = 0.4;
vector<solution_t> population;
vector<int> fitness;
vector<double> selection;
vector<int> selected_for_reproduction;
vector<int> harem;
vector<solution_t> babies;

solution_t random_solution()
{
	return solution_t(rand() % 32);
}

void selectHarem()
{
	harem.clear();
	for each (auto var in selected_for_reproduction)
	{
		if ((rand() % 100000) / 100000.0 < sex_probability)
			harem.push_back(var);
	}
}

solution_t make_baby(solution_t father, solution_t mother)
{
	auto pivot = (rand() % 4) + 1;
	//pivot = 4;
	cout << pivot << endl;
	auto bits_from_father = (father >> pivot) << pivot;
	auto bits_from_mother = mother & solution_t(_Pow_int(2, pivot) - 1);
	cout << bits_from_father << endl;
	cout << bits_from_mother << endl;


	auto baby = bits_from_father | bits_from_mother;
	return solution_t(baby);
}

solution_t make_baby(int a, int b)
{
	auto father = population[a];
	auto mother = population[b];
	return make_baby(father, mother);
}


void solve_demographic_problem()
{
	babies.clear();
	for each (auto var in harem)
	{
		auto partner = rand() % harem.size();

		auto baby = make_baby(var, partner);
		babies.push_back(baby);
	}


}

long double f(int x)
{
	return 2 * x + 1;
}

vector<solution_t>

int wheel_the_roulette(int fitness_sum)
{
	auto pick = (rand() % 100000) / 100000.0;
	auto i = 0;
	while (pick > 0)
	{
		pick -= selection[i];
		i++;
	}
	assert(i <= fitness.size());
	return i - 1;
}

bool GetProbability(double p)
{
	return (rand() % 100000) / 100000.0 < p;
}

void chernobyl_population(vector<solution_t> &population)
{
	for each (auto item in population)
	{
		if (GetProbability(mutation_probability))
		{
			auto n = item.flip(rand() % 5);
		}
	}
}

void print_vec(const vector<solution_t> &vec, string title)
{
	cout << title << endl;
	int i = 0;
	for each (auto item in vec)
	{
		cout << i++ << ": " << item << "\tfitness:" << f((int) item.to_ulong())  << endl;
	}
}

int main()
{
	srand(17);
	for (size_t i = 0; i < 8; i++)
	{
		population.push_back(random_solution());
	}

	for each (auto item in population)
	{
		cout << item << endl;
	}

	auto sumOfAllfitness = 0.0;
	for each (auto var in population)
	{
		auto c = (f((int)var.to_ulong()));
		sumOfAllfitness += c;
		fitness.push_back(c);
	}
	for each (auto item in fitness)
	{
		cout << item << endl;
	}
	cout << "Sum of all fitness: " << sumOfAllfitness << endl;

	for each (auto var in fitness)
	{
		selection.push_back(var * 1.0 / sumOfAllfitness);
	}

	for each (auto var in selection)
	{
		cout << var *100.0 << "%\n";
	}

	for (size_t i = 0; i < 6; i++)
	{
		auto dd = wheel_the_roulette(sumOfAllfitness);
		selected_for_reproduction.push_back(dd);
		cout << dd << endl;
	}

	selectHarem();
	solve_demographic_problem();

	auto wholeOfNewPopulation = vector<solution_t>();
	wholeOfNewPopulation.insert(wholeOfNewPopulation.end(), babies.begin(), babies.end());
	wholeOfNewPopulation.insert(wholeOfNewPopulation.end(), population.begin(), population.end());

	print_vec(wholeOfNewPopulation, "Population after reproduction");
	chernobyl_population(wholeOfNewPopulation);
	print_vec(wholeOfNewPopulation, "After mutation");




	//cout << "PORN\n";

	//auto mother = solution_t("11111");
	//auto father = solution_t("00000");
	//auto baby = make_baby( father, mother);
	//cout << baby;
		


	cin.ignore();
	cin.get();
	return 0;
}
