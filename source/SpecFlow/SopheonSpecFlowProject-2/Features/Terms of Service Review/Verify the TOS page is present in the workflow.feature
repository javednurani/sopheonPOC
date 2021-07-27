Feature: Verify the TOS page is present in the workflow
    @TestCaseKey=CLOUD-T45
    Scenario: Verify the TOS page is present in the workflow
        
        Given the user is in the New Account Sign Up workflow
        
        When the user clicks on Create
        
        Then the user is taken to the TOS page