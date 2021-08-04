Feature: Verify Cell Phone is optional
    @TestCaseKey=CLOUD-T36
    Scenario: Verify Cell Phone is optional
        
        Given the user is on the PL Account setup page
        
        When the user creates an account without a cell phone
        
        Then the account is created 