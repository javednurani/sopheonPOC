Feature: Verify Account Password is Required on page load
    @TestCaseKey=VerifyAccountPasswordIsRequiredOnPageLoad
    Scenario: Verify Account Password is Required on page load
        
        Given the user is on the PL Account Setup Page
        
        When loads the page 
        
        Then the Account Password field is marked as required