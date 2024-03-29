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
  public void ShouldGenerateInlineParametersCorrectly()
  { 
    InvokeThenAssert("inlineParameter", new []{"-inlineRequest"});
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
  public void ShouldEnumUseEnum()
  { 
    InvokeThenAssert(dir:"enumUseEnum", new []{"-eUe"});
  }  

    [Test]
  public void ShouldParameterAllof()
  { 
    InvokeThenAssert(dir:"parameterAllOf");
  }  
  
  [Test]
  public void ShouldAllof()
  { 
    InvokeThenAssert(dir:"allof");
  }
  
    
  [Test]
  public void ShouldNonNullResponseAsRequired()
  { 
    InvokeThenAssert(dir:"nonNullResponseAsRequired", new []{"-nnr"});
  }
  
  [Test]
  public void ShouldNullAsOptional()
  { 
    InvokeThenAssert(dir:"nullAsOptional", new []{"-nullAsOptional"});
  }
}