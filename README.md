# ByteFile [![Build Status](https://travis-ci.org/Ryozuki/ByteFile.svg?branch=master)](https://travis-ci.org/Ryozuki/ByteFile)
A library to handle raw file reading and writing.

## Usage
```csharp
BFile MyFile = new BFile("file.dat", true);

var old_encoding = MyFile.GetEncoding(); // Default is UTF8
MyFile.SetEncoding(Encoding.ASCII);


// Write
MyFile.Write(
	1, 
	1.2f, 
	134151513151UL, 
	false, 
	"hello world",
	);

// Read
var a1 = MyFile.Read<int>();
var a2 = MyFile.Read<float>();
var a3 = MyFile.Read<ulong>();
var a4 = MyFile.Read<bool>();
var a5 = MyFile.Read<string>();
```