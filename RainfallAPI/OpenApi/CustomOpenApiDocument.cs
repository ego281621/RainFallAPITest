using System;
using System.Collections.Generic;

namespace Microsoft.OpenApi.Models
{
    public class CustomOpenApiDocument : OpenApiDocument
    {
        public CustomOpenApiDocument()
        {
            InitializeWithRainfallApiSpecification();
        }

        private void InitializeWithRainfallApiSpecification()
        {
            Info = new OpenApiInfo
            {
                Title = "Rainfall Api",
                Version = "1.0",
                Contact = new OpenApiContact
                {
                    Name = "Sorted",
                    Url = new Uri("https://www.sorted.com")
                },
                Description = "An API which provides rainfall reading data"
            };

            Servers = new List<OpenApiServer>
            {
                new OpenApiServer
                {
                    Url = "http://localhost:3000",
                    Description = "Rainfall Api"
                }
            };

            Tags = new List<OpenApiTag>
            {
                new OpenApiTag
                {
                    Name = "Rainfall",
                    Description = "Operations relating to rainfall"
                }
            };

            Paths = new OpenApiPaths
            {
                ["/rainfall/id/{stationId}/readings"] = new OpenApiPathItem
                {
                    Parameters = new List<OpenApiParameter>
                    {
                        new OpenApiParameter
                        {
                            Schema = new OpenApiSchema
                            {
                                Type = "string"
                            },
                            Name = "stationId",
                            In = ParameterLocation.Path,
                            Required = true,
                            Description = "The id of the reading station"
                        },
                        new OpenApiParameter
                        {
                            Schema = new OpenApiSchema
                            {
                                Type = "number",
                                Minimum = 1,
                                Maximum = 100,
                                //Default = 10
                            },
                            Name = "count",
                            In = ParameterLocation.Query,
                            Required = false,
                            Description = "The number of readings to return"
                        }
                    },
                    Operations = new Dictionary<OperationType, OpenApiOperation>
                    {
                        [OperationType.Get] = new OpenApiOperation
                        {
                            OperationId = "get-rainfall",
                            Summary = "Get rainfall readings by station Id",
                            Description = "Retrieve the latest readings for the specified stationId",
                            Tags = new List<OpenApiTag>
                            {
                                new OpenApiTag { Name = "Rainfall" }
                            },
                            Responses = new OpenApiResponses
                            {
                                ["200"] = new OpenApiResponse
                                {
                                    Description = "A list of rainfall readings successfully retrieved",
                                    Content = new Dictionary<string, OpenApiMediaType>
                                    {
                                        ["application/json"] = new OpenApiMediaType
                                        {
                                            Schema = new OpenApiSchema
                                            {
                                                Reference = new OpenApiReference
                                                {
                                                    Type = ReferenceType.Schema,
                                                    Id = "#/components/schemas/rainfallReadingResponse"
                                                }
                                            }
                                        }
                                    }
                                },
                                ["400"] = new OpenApiResponse
                                {
                                    Description = "Invalid request",
                                    Content = new Dictionary<string, OpenApiMediaType>
                                    {
                                        ["application/json"] = new OpenApiMediaType
                                        {
                                            Schema = new OpenApiSchema
                                            {
                                                Reference = new OpenApiReference
                                                {
                                                    Type = ReferenceType.Schema,
                                                    Id = "#/components/schemas/error"
                                                }
                                            }
                                        }
                                    }
                                },
                                ["404"] = new OpenApiResponse
                                {
                                    Description = "No readings found for the specified stationId",
                                    Content = new Dictionary<string, OpenApiMediaType>
                                    {
                                        ["application/json"] = new OpenApiMediaType
                                        {
                                            Schema = new OpenApiSchema
                                            {
                                                Reference = new OpenApiReference
                                                {
                                                    Type = ReferenceType.Schema,
                                                    Id = "#/components/schemas/error"
                                                }
                                            }
                                        }
                                    }
                                },
                                ["500"] = new OpenApiResponse
                                {
                                    Description = "Internal server error",
                                    Content = new Dictionary<string, OpenApiMediaType>
                                    {
                                        ["application/json"] = new OpenApiMediaType
                                        {
                                            Schema = new OpenApiSchema
                                            {
                                                Reference = new OpenApiReference
                                                {
                                                    Type = ReferenceType.Schema,
                                                    Id = "#/components/schemas/error"
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            Components = new OpenApiComponents
            {
                Schemas = new Dictionary<string, OpenApiSchema>
                {
                    ["rainfallReadingResponse"] = new OpenApiSchema
                    {
                        Title = "Rainfall reading response",
                        Type = "object",
                        Description = "Details of a rainfall reading",
                        Properties = new Dictionary<string, OpenApiSchema>
                        {
                            ["readings"] = new OpenApiSchema
                            {
                                Type = "array",
                                Items = new OpenApiSchema
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.Schema,
                                        Id = "#/components/schemas/rainfallReading"
                                    }
                                }
                            }
                        }
                    },
                    ["rainfallReading"] = new OpenApiSchema
                    {
                        Title = "Rainfall reading",
                        Type = "object",
                        Description = "Details of a rainfall reading",
                        Properties = new Dictionary<string, OpenApiSchema>
                        {
                            ["dateMeasured"] = new OpenApiSchema
                            {
                                Type = "string",
                                Format = "date-time"
                            },
                            ["amountMeasured"] = new OpenApiSchema
                            {
                                Type = "number",
                                Format = "decimal"
                            }
                        }
                    },
                    ["error"] = new OpenApiSchema
                    {
                        Title = "Error response",
                        Type = "object",
                        Description = "Details of a rainfall reading",
                        Properties = new Dictionary<string, OpenApiSchema>
                        {
                            ["message"] = new OpenApiSchema
                            {
                                Type = "string"
                            },
                            ["detail"] = new OpenApiSchema
                            {
                                Items = new OpenApiSchema
                                {
                                    Type = "array",
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.Schema,
                                        Id = "#/components/schemas/errorDetail"
                                    }
                                },
                                AdditionalPropertiesAllowed = false
                            }
                        }
                    },
                    ["errorDetail"] = new OpenApiSchema
                    {
                        Type = "object",
                        Description = "Details of invalid request property",
                        Properties = new Dictionary<string, OpenApiSchema>
                        {
                            ["propertyName"] = new OpenApiSchema
                            {
                                Type = "string"
                            },
                            ["message"] = new OpenApiSchema
                            {
                                Type = "string"
                            }
                        },
                        AdditionalPropertiesAllowed = false
                    }
                },
                Responses = new Dictionary<string, OpenApiResponse>
                {
                    ["rainfallReadingResponse"] = new OpenApiResponse
                    {
                        Description = "Get rainfall readings response",
                        Content = new Dictionary<string, OpenApiMediaType>
                        {
                            ["application/json"] = new OpenApiMediaType
                            {
                                Schema = new OpenApiSchema
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.Schema,
                                        Id = "#/components/schemas/rainfallReadingResponse"
                                    }
                                }
                            }
                        }
                    },
                    ["errorResponse"] = new OpenApiResponse
                    {
                        Description = "An error object returned for failed requests",
                        Content = new Dictionary<string, OpenApiMediaType>
                        {
                            ["application/json"] = new OpenApiMediaType
                            {
                                Schema = new OpenApiSchema
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.Schema,
                                        Id = "#/components/schemas/error"
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}
