using Ambev.DeveloperEvaluation.Domain.Entities;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    /// <summary>
    /// Contains unit tests for the Branch entity class.
    /// Tests cover name setting and product list handling.
    /// </summary>
    public class BranchTests
    {
        /// <summary>
        /// Tests that a new branch can be created with a valid name.
        /// </summary>
        [Fact(DisplayName = "Branch should be created with a valid name")]
        public void Given_ValidBranchName_When_Created_Then_BranchShouldHaveCorrectName()
        {
            // Arrange
            var branchName = "Main Branch";

            // Act
            var branch = new Branch(branchName);

            // Assert
            Assert.Equal(branchName, branch.Name);
        }

        /// <summary>
        /// Tests that the branch name can be updated.
        /// </summary>
        [Fact(DisplayName = "Branch name should be updated")]
        public void Given_Branch_When_NameSet_Then_BranchNameShouldBeUpdated()
        {
            // Arrange
            var branch = new Branch("Old Branch Name");
            var newBranchName = "Updated Branch Name";

            // Act
            branch.SetName(newBranchName);

            // Assert
            Assert.Equal(newBranchName, branch.Name);
        }

        /// <summary>
        /// Tests that a branch starts with an empty list of products.
        /// </summary>
        [Fact(DisplayName = "Branch should start with an empty product list")]
        public void Given_NewBranch_When_Created_Then_ProductListShouldBeEmpty()
        {
            // Arrange
            var branch = new Branch("Test Branch");

            // Act
            var productList = branch.Products;

            // Assert
            Assert.NotNull(productList);
            Assert.Empty(productList);
        }

        /// <summary>
        /// Tests that a product can be added to the branch's product list.
        /// </summary>
        [Fact(DisplayName = "Product should be added to branch's product list")]
        public void Given_ValidProduct_When_AddedToBranch_Then_ProductListShouldContainProduct()
        {
            // Arrange
            var branch = new Branch("Test Branch");
            var product = new Product("Test Product", 100.00m, branch.Id); 

            // Act
            branch.Products.Add(product);

            // Assert
            Assert.Contains(product, branch.Products);
            Assert.Single(branch.Products); // Since we added only one product
        }

    }
}
