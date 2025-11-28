// <copyright file="AppState.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.Services;

using System.Globalization;
using GSD.Minecraft.Plotter.Models;
using GSD.Minecraft.Plotter.ViewModels;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Represents the application state, providing properties and functionality to manage and interact with the current state of the application.
/// </summary>
public class AppState
{
    /// <summary>
    /// The key used to identify the setting for the current world ID in the application's database.
    /// </summary>
    private const string CurrentWorldIdSettingKey = "CurrentWorldId";

    /// <summary>
    /// The factory for creating instances of <see cref="AppDbContext" />, used to manage and interact with the application's database.
    /// </summary>
    private readonly IDbContextFactory<AppDbContext> dbContextFactory;

    /// <summary>
    /// The current world.
    /// </summary>
    private World currentWorld;

    /// <summary>
    /// The list of markers for the current world.
    /// </summary>
    private IList<Marker> currentWorldMarkers;

    /// <summary>
    /// Indicates whether the application state has been initialized.
    /// </summary>
    private bool isInitialized;

    /// <summary>
    /// The list of worlds.
    /// </summary>
    private IList<World> worlds;

    /// <summary>
    /// Initializes a new instance of the <see cref="AppState" /> class with the specified database context.
    /// </summary>
    /// <param name="dbContextFactory">The factory for creating instances of <see cref="AppDbContext" />, used to manage and interact with the application's database.</param>
    public AppState(IDbContextFactory<AppDbContext> dbContextFactory)
    {
        this.dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
    }

    /// <summary>
    /// Occurs when the current world in the application state changes.
    /// </summary>
    public event EventHandler CurrentWorldChanged;

    /// <summary>
    /// Occurs when the collection of markers in the application state is modified.
    /// </summary>
    public event EventHandler MarkersChanged;

    /// <summary>
    /// Occurs when the collection of worlds in the application state is modified.
    /// </summary>
    public event EventHandler WorldsChanged;

    /// <summary>
    /// Adds a new marker or updates an existing marker in the application's state asynchronously.
    /// </summary>
    /// <param name="marker">The <see cref="Marker" /> instance representing the marker to add or update.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> that can be used to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public async Task AddOrUpdateMarkerAsync(Marker marker, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(marker);

        var dbContext = await this.EnsureInitializedAsync(cancellationToken).ConfigureAwait(false);

        marker.World = await this.GetCurrentWorldAsync(dbContext, cancellationToken).ConfigureAwait(false);

        var existingMarker = await dbContext.Markers.FindAsync([marker.Id], cancellationToken).ConfigureAwait(false);

        if (existingMarker != null)
        {
            dbContext.Entry(existingMarker).CurrentValues.SetValues(marker);
        }
        else
        {
            dbContext.Markers.Add(marker);
        }

        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        this.currentWorldMarkers = null;
        this.OnMarkersChanged();
    }

    /// <summary>
    /// Adds a new world or updates an existing world in the application's state asynchronously.
    /// </summary>
    /// <param name="world">The <see cref="World" /> instance representing the world to add or update.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> that can be used to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public async Task AddOrUpdateWorldAsync(World world, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(world);

        var dbContext = await this.EnsureInitializedAsync(cancellationToken).ConfigureAwait(false);

        var existingWorld = await dbContext.Worlds.FindAsync([world.Id], cancellationToken).ConfigureAwait(false);

        if (existingWorld != null)
        {
            dbContext.Entry(existingWorld).CurrentValues.SetValues(world);
        }
        else
        {
            dbContext.Worlds.Add(world);
        }

        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        await SetCurrentWorldIdAsync(dbContext, world.Id, cancellationToken).ConfigureAwait(false);

        this.worlds = null;
        this.currentWorld = null;
        this.currentWorldMarkers = null;
        this.OnWorldsChanged();
        this.OnCurrentWorldChanged();
        this.OnMarkersChanged();
    }

    /// <summary>
    /// Deletes a marker with the specified identifier from the application's state asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the marker to delete.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> that can be used to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public async Task DeleteMarkerAsync(int id, CancellationToken cancellationToken = default)
    {
        var dbContext = await this.EnsureInitializedAsync(cancellationToken).ConfigureAwait(false);

        var marker = await dbContext.Markers
            .FindAsync([id], cancellationToken)
            .ConfigureAwait(false);

        if (marker == null)
        {
            return;
        }

        dbContext.Markers.Remove(marker);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        this.currentWorldMarkers = null;
        this.OnMarkersChanged();
    }

    /// <summary>
    /// Deletes a world with the specified identifier from the application's state asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the world to delete.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> that can be used to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public async Task DeleteWorldAsync(int id, CancellationToken cancellationToken = default)
    {
        var dbContext = await this.EnsureInitializedAsync(cancellationToken).ConfigureAwait(false);

        var world = await dbContext.Worlds
            .FindAsync([id], cancellationToken)
            .ConfigureAwait(false);

        if (world == null)
        {
            return;
        }

        dbContext.Worlds.Remove(world);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        if (this.currentWorld?.Id == world.Id)
        {
            var newWorld = await dbContext.Worlds
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);

            if (newWorld != null)
            {
                await SetCurrentWorldIdAsync(dbContext, newWorld.Id, cancellationToken).ConfigureAwait(false);
            }

            this.worlds = null;
            this.currentWorld = null;
            this.currentWorldMarkers = null;
            this.OnWorldsChanged();
            this.OnCurrentWorldChanged();
            this.OnMarkersChanged();
        }
        else
        {
            this.worlds = null;
            this.OnWorldsChanged();
        }
    }

    /// <summary>
    /// Retrieves the current world from the application's state asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> that can be used to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="Task{TResult}" /> representing the asynchronous operation, with a result of type <see cref="World" /> representing the current world.</returns>
    public async Task<World> GetCurrentWorldAsync(CancellationToken cancellationToken = default)
    {
        if (this.currentWorld != null)
        {
            return this.currentWorld;
        }

        var dbContext = await this.EnsureInitializedAsync(cancellationToken).ConfigureAwait(false);
        return await this.GetCurrentWorldAsync(dbContext, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously retrieves a list of markers associated with the current world from the application's state.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> that can be used to cancel the asynchronous operation.</param>
    /// <returns>
    /// A <see cref="Task{TResult}" /> representing the asynchronous operation, with a result of an <see cref="IList{Marker}" /> containing the markers.
    /// </returns>
    public async Task<IList<Marker>> GetMarkersAsync(CancellationToken cancellationToken = default)
    {
        this.currentWorldMarkers ??= await Factory().ConfigureAwait(false);
        return this.currentWorldMarkers;

        async Task<IList<Marker>> Factory()
        {
            var dbContext = await this.EnsureInitializedAsync(cancellationToken).ConfigureAwait(false);

            var world = await this.GetCurrentWorldAsync(dbContext, cancellationToken).ConfigureAwait(false);

            if (world == null)
            {
                return [];
            }

            return await dbContext.Markers
                .Where(m => m.WorldId == world.Id)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Retrieves an asynchronous stream of all worlds in the application.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> that can be used to cancel the asynchronous operation.</param>
    /// <returns>
    /// An <see cref="IAsyncEnumerable{T}" /> of <see cref="WorldViewModel" /> representing the worlds.
    /// </returns>
    public async Task<IList<World>> GetWorldsAsync(CancellationToken cancellationToken = default)
    {
        this.worlds ??= await Factory().ConfigureAwait(false);
        return this.worlds;

        async Task<IList<World>> Factory()
        {
            var dbContext = await this.EnsureInitializedAsync(cancellationToken).ConfigureAwait(false);

            var results = await dbContext.Worlds
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            if (results.Count == 0)
            {
                var world = await CreateDefaultWorldAsync(dbContext, cancellationToken).ConfigureAwait(false);
                return [world];
            }

            return results;
        }
    }

    /// <summary>
    /// Raises the <see cref="CurrentWorldChanged" /> event to notify subscribers that the current world has changed.
    /// </summary>
    protected virtual void OnCurrentWorldChanged()
    {
        this.CurrentWorldChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Raises the <see cref="MarkersChanged" /> event to notify subscribers that the collection of markers has changed.
    /// </summary>
    protected virtual void OnMarkersChanged()
    {
        this.MarkersChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Raises the <see cref="WorldsChanged" /> event to notify subscribers that the collection of worlds has changed.
    /// </summary>
    protected virtual void OnWorldsChanged()
    {
        this.WorldsChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Creates a default world with predefined settings and saves it to the database.
    /// </summary>
    /// <param name="dbContext">The <see cref="AppDbContext" />, used to manage and interact with the application's database.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> that can be used to cancel the asynchronous operation.</param>
    /// <returns>
    /// A <see cref="Task" /> representing any asynchronous operation. The task result contains the created <see cref="World" /> instance.
    /// </returns>
    private static async Task<World> CreateDefaultWorldAsync(AppDbContext dbContext, CancellationToken cancellationToken)
    {
        var defaultWorld = new World
        {
            Name = "Default World",
        };

        dbContext.Worlds.Add(defaultWorld);

        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        await SetCurrentWorldIdAsync(dbContext, defaultWorld.Id, cancellationToken).ConfigureAwait(false);

        return defaultWorld;
    }

    /// <summary>
    /// Sets the current world ID in the application's database.
    /// </summary>
    /// <param name="dbContext">The <see cref="AppDbContext" />, used to manage and interact with the application's database.</param>
    /// <param name="worldId">The ID of the world to set as the current world.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> that can be used to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous operation whose result is the <see cref="AppDbContext" />, used to manage and interact with the application's database.</returns>
    private static async Task SetCurrentWorldIdAsync(AppDbContext dbContext, int worldId, CancellationToken cancellationToken)
    {
        var worldIdSetting = await dbContext.Settings
            .FindAsync([CurrentWorldIdSettingKey], cancellationToken)
            .ConfigureAwait(false);

        if (worldIdSetting == null)
        {
            worldIdSetting = new Setting
            {
                Key = CurrentWorldIdSettingKey,
                Value = worldId.ToString(CultureInfo.InvariantCulture),
            };

            dbContext.Settings.Add(worldIdSetting);
        }
        else
        {
            worldIdSetting.Value = worldId.ToString(CultureInfo.InvariantCulture);
        }

        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously initializes the application state, performing any necessary setup or loading operations.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> that can be used to cancel the asynchronous operation.</param>
    /// <returns>
    /// A <see cref="Task" /> representing the asynchronous initialization operation whose result is a <see cref="AppDbContext" />, used to manage and interact with the application's database.
    /// </returns>
    private async Task<AppDbContext> EnsureInitializedAsync(CancellationToken cancellationToken = default)
    {
        var dbContext = await this.dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        if (this.isInitialized)
        {
            return dbContext;
        }

        this.isInitialized = true;

        if (await dbContext.Database.EnsureCreatedAsync(cancellationToken).ConfigureAwait(false))
        {
            var databaseVersion = new Setting
            {
                Key = "DatabaseVersion",
                Value = new Version(1, 0, 0, 0).ToString(),
            };

            dbContext.Settings.Add(databaseVersion);

            await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await CreateDefaultWorldAsync(dbContext, cancellationToken).ConfigureAwait(false);
        }

        return dbContext;
    }

    /// <summary>
    /// Retrieves the current world from the application's state asynchronously.
    /// </summary>
    /// <param name="dbContext">The <see cref="AppDbContext" />, used to manage and interact with the application's database.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> that can be used to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="Task{TResult}" /> representing the asynchronous operation, with a result of type <see cref="World" /> representing the current world.</returns>
    private async Task<World> GetCurrentWorldAsync(AppDbContext dbContext, CancellationToken cancellationToken = default)
    {
        this.currentWorld ??= await Factory().ConfigureAwait(false);
        return this.currentWorld;

        async Task<World> Factory()
        {
            var worldIdSetting = await dbContext.Settings
                .FindAsync([CurrentWorldIdSettingKey], cancellationToken)
                .ConfigureAwait(false);

            if ((worldIdSetting == null) || !int.TryParse(worldIdSetting.Value, out var worldId))
            {
                return await CreateDefaultWorldAsync(dbContext, cancellationToken).ConfigureAwait(false);
            }

            var world = await dbContext.Worlds
                .FindAsync([worldId], cancellationToken)
                .ConfigureAwait(false);

            if (world == null)
            {
                return await CreateDefaultWorldAsync(dbContext, cancellationToken).ConfigureAwait(false);
            }

            return world;
        }
    }
}