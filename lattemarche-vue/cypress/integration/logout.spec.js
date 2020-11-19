describe('Logout', () => {

    it('Logout', () => {    
      
      cy.login();
      
      cy.logout();
  
      cy.url().should('eq', Cypress.env('url') + '/')
  
    })
  
  })
  