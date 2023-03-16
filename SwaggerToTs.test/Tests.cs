namespace SwaggerToTs.test;

public class Test:TestBase
{

  [Test]
  public void ShouldGenerateRoutesCorrectly()
  { 
    InvokeThenAssert("Routes");
  }
  
  [Test]
  public void ShouldMergeParameterCorrectly()
  { 
    InvokeThenAssert("pathItemParameters");
  }
  
  [Test]
  public void ShouldShowMethodCorrectly()
  { 
    InvokeThenAssert("pathItemMethod");
  }  
  
  [Test]
  public void DuplicateExportNameShouldAddNumberToEnd()
  { 
    InvokeThenAssert("operationExportNameAndFile");
  }
  
  [Test]
  public void ShouldGenerateParametersCorrectly()
  { 
    InvokeThenAssert("parameter");
  }
  
  
  [Test]
  public void ShouldGenerateRequestBodyCorrectly()
  { 
    InvokeThenAssert("requestBody");
  }
  
   
  [Test]
  public void ShouldGenerateCorrectlyWhenRequestBodyIsEmpty()
  { 
    InvokeThenAssert("requestBodyEmpty");
  }
  
  
  [Test]
  public void ShouldGenerateCorrectlyWhenRequestBodyEmptyRef()
  { 
    InvokeThenAssert("requestBodyEmptyRef");
  }
  
  [Test]
  public void ShouldGenerateCorrectlyWhenRequestBodyRef()
  { 
    InvokeThenAssert("requestBodyRef");
  }

    
  [Test]
  public void ShouldGenerateCorrectlyWhenRequestbodyRefRequired()
  { 
    InvokeThenAssert("requestbodyRefRequired");
  }
  
     
  [Test]
  public void ShouldGenerateCorrectlyWhenRequestbodyRequired()
  { 
    InvokeThenAssert("requestbodyRequired");
  }
  
  [Test]
  public void ShouldGenerateCorrectlyWhenParameterEmpty()
  { 
    InvokeThenAssert("parameterEmpty");
  }
  
  [Test]
  public void ShouldGenerateCorrectlyWhenParameterIncludesRef()
  { 
    InvokeThenAssert("parameterIncludesRef");
  }
  
    
  [Test]
  public void ShouldGenerateCorrectlyWhenParameterIncludeRef()
  { 
    InvokeThenAssert("parameterIncludeRef");
  } 
  
  [Test]
  public void ShouldGenerateResponseHeaderAndRefCorrectly()
  { 
    InvokeThenAssert("responseHeaderAndRef");
  }  
  
  [Test]
  public void ShouldGenerateBaseSchemaCorrectly()
  { 
    InvokeThenAssert("schemaBase");
  }
  
  [Test]
  public void ShouldGenerateSchemaComposeCorrectly()
  { 
    InvokeThenAssert("schemaCompose");
  }  
  
  [Test]
  public void ShouldGenerateSchemaComposeTypeCorrectly()
  { 
    InvokeThenAssert("schemaComposeType");
  }  
  
  [Test]
  public void ShouldGuessNullableCorrectly()
  { 
    InvokeThenAssert(dir:"GuessNullable", new []{"-g"});
  }
  
    
  [Test]
  public void ShouldNullValueIgnoreCorrectly()
  { 
    InvokeThenAssert(dir:"NullValueIgnore", new []{"-n"});
  }
  
  [Test]
  public void ShouldNullValueIgnoreAndGuessCorrectly()
  { 
    InvokeThenAssert(dir:"NullValueIgnoreAndGuess", new []{"-g", "-n"});
  }
  
  [Test]
  public void ShouldSaveToCommonCorrectly()
  { 
    InvokeThenAssert(dir:"SaveToCommon", new []{"-s"});
  }
}