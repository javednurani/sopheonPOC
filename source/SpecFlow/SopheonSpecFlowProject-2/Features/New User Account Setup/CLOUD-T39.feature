Feature: Verify Creating an account with Marketing Newsletter checked
    @TestCaseKey=CLOUD-T39
    Scenario: Verify Creating an account with Marketing Newsletter checked
        
        Given the user is on the PL Account Setup Page
        
        When the user submits an account with the Marketing Newsletter Checked
        
        Then the Account is created