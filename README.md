# DataConveyer_ConvertCsvToFlat

DataConveyer_ConvertCsvToFlat is a sample console application to convert a CSV file to a flat (fixed-width field) file.
It uses Data Conveyer to perform the conversion.

The input file is expected to contain aircraft data along with IATA and ICAO codes. Like so:

```
"Airbus A320","320","A320"
"Boeing 747","747",\N
```

There is a sample input file (200+ lines, 8+KB) included in ...Data folder. The file is a copy of
[planes.dat](https://github.com/jpatokal/openflights/blob/master/data/planes.dat) file, which is
licensed under [Open Database License](https://github.com/jpatokal/openflights/blob/master/data/LICENSE).

Upon execution, the resulting output file will contan translations from the IATA code into the aircraft names (ICAO codes get discarded).
Like so:

```
320 - Airbus A320
747 - Boeing 747
```

## Installation

* Fork this repository and clone it onto your local machine, or

* Download this repository onto your local machine.

## Usage

1. Open DataConveyer_ConvertCsvToFlat solution in Visual Studio.

2. Build and run the application, e.g. hit F5

    - a console window with directions will show up.

3. Copy an input file (e.g. planes.dat from  ...Data folder) into the ...Data\In folder

    - the file will get processed as reported in the console window.

4. Review the contents of the output file placed in the ...Data\Out folder.

5. (optional) Repeat steps 3-4 for other additional input file(s).

6. To exit application, hit Enter key into the console window.

## Contributing  

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License

[Apache License 2.0](https://choosealicense.com/licenses/apache-2.0/)

## Copyright

```
Copyright © 2019 Mavidian Technologies Limited Liability Company. All Rights Reserved.
```
