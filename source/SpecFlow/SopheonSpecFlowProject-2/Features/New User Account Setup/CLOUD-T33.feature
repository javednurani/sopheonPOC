Feature: Verify First Name is Required on page load
    @TestCaseKey=CLOUD-T33
    Scenario: Verify First Name is Required on page load
        
        Given the user is on the PL Account Setup Page
        
        When loads the page 
        
        Then the First Name field is marked as required