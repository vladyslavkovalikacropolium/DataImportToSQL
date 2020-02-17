

#Configurations 
DataProvider
  SpecificationPath - path to file with specifications for data import from flat file. 
                        Specifications should be in json format and looks like 
```JSON
{
                          "TableName" : "TableName",
                          "FileType": {
                            "FixedLength": true,
                            "QuotedText": false,
                            "FieldsDelimiter": ",",
                            "RecordsDelimiter": "\n"
                          },
                          "Specifications": [
                            {
                              "FieldName": "Name",
                              "Beginning": 1,
                              "FieldLength": 400
                            },
                          ]
 }
```
                        
   
  FilesPath - path to folder with flat files
  
  
#Description 
We decided to use a separate table for each specification of flat files.
Based on the specification, we create the table, then read the data in blocks, parse them and add to data set.
In final we call BulkSave and fast save data from flat file to data base table.

Saving works optimally and saves 1,500,000 records in 20s.


Optimization and more complex architecture require more time.
