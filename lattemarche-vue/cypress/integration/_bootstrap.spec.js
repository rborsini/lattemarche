describe('Bootstrap', () => {

    it('Site open', () => {    
      
        cy.visit(Cypress.env('url'), { timeout: 600000 });
  
    })
  
  })
  