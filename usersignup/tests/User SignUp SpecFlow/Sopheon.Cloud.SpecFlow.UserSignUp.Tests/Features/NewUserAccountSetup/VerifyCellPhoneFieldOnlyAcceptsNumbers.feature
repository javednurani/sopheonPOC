Feature: Verify Cell Phone field only accepts numbers
    @TestCaseKey=CLOUD-T37
    Scenario: Verify Cell Phone field only accepts numbers
        
        Given the user is on the PL Account Setup Page
        
        When the user adds non numeric characters to the cell phone field
        
        Then the account is not created