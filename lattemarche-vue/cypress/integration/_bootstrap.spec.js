describe('Bootstrap', () => {

    var retryObj = {
        retries: {
            runMode: 5,
            openMode: 3
        }
    };

    it('Site open', retryObj, () => {    
      
        cy.visit(Cypress.env('url'), { timeout: 600000 });
  
    })
  
  })
  