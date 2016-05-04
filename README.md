# mt19937_64_cs
C++11 std::mt19937_64 for C#

Now you can use same random algorithm on C++ and C#.

It return same result with same seed value.

It was tested on vs2015.


[C++]

long long seed = 123456;

std::mt19937_64 gen(seed);

std::uniform_int_distribution<long long> dis(1, 10000);

auto result = dis(gen);


[C#]

long seed = 123456;

std.mt19937_64 gen;

gen.init(seed);

long result = (gen.next() % 10000)+1;
