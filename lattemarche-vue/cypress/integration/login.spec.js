describe('Login', () => {

    it('Login', () => {    
      
      cy.login();
      cy.get('.cy-username').should('contain', Cypress.env('username'));
  
    })
  
  })
  