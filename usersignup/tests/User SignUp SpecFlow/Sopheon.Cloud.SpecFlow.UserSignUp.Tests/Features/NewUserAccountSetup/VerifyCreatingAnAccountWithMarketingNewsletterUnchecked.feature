Feature: Verify Creating an account with Marketing Newsletter unchecked
    @TestCaseKey=CLOUD-T40
    Scenario: Verify Creating an account with Marketing Newsletter unchecked
        
        Given the user is on the PL Account Setup Page
        
        When the user submits an account with the Marketing Newsletter unchecked
        
        Then the Account is created