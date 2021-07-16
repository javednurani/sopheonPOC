Feature: Verify marketing newsletter checkbox is checked
    @TestCaseKey=CLOUD-T38
    Scenario: Verify marketing newsletter checkbox is checked
        
        Given the user is on the PL Account Setup Page
        
        When the page loads
        
        Then the Marketing Newsletter Checkbox is checked